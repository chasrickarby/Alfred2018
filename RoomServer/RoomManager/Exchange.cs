///-----------------------------------------------------------------
/// <summary>
/// Communication with Microsoft EWS (Exchange Web Service).
/// </summary>
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Exchange.WebServices.Data;

namespace RoomManager
{
    public class Exchange : IExchange
    {
        private readonly string username = "alfredoa@ptc.com";
        private readonly string password = "Ptc!1234";
        private readonly string exchangeUrl = "https://outlook.office365.com/ews/exchange.asmx";
        private readonly ExchangeVersion exchangeVersion = ExchangeVersion.Exchange2010;
        private const string roomFilter = "";
        private const string addressDomain = "@ptc.com";
        private const string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Alfred;Persist Security Info=True;User ID=Alfred;Password=The1andonly!";

        public Exchange()
        {
        }

        public IEnumerable<Room> GetAllRoomsDetails()
        {
            return GetAllRooms(roomFilter);
        }

        public IEnumerable<Room> GetAppointmentsAllRooms(DateTime start, DateTime end)
        {
            var rooms = GetAllRooms(roomFilter).ToArray();
            for (int i = 0; i < rooms.Length; i++)
            {
                GetRoomAppointments(ref rooms[i], start, end);
                GetRoomWeather(ref rooms[i]);
            }

            return rooms;
        }

        public Room GetAppointmentsByRoomAddress(string roomAddress, DateTime start, DateTime end)
        {
            var room = new Room
            {
                Address = ValidateRoomAddress(roomAddress)
        };
            GetRoomAppointments(ref room, start, end);
            GetRoomWeather(ref room);
            return room;
        }

        public Response SendMeetingRequest(string roomAddress, string subject, DateTime start, DateTime end)
        {
            try
            {
                roomAddress = ValidateRoomAddress(roomAddress);
                if(string.IsNullOrEmpty(subject))
                {
                    subject = "Impromptu meeting";
                }

                Appointment meeting = new Appointment(Service)
                {
                    // Set the properties on the meeting object to create the meeting.
                    Subject = subject,
                    Body = $"Meeting auto created by Alfred room manager",
                    Start = start,
                    End = end,
                    Location = roomAddress
                };
                meeting.RequiredAttendees.Add(username);
                meeting.RequiredAttendees.Add("sgile@ptc.com");
                meeting.RequiredAttendees.Add("mwestover@ptc.com");
                meeting.RequiredAttendees.Add(roomAddress);
                meeting.ReminderMinutesBeforeStart = 1;

                // Save the meeting to the Calendar folder and send the meeting request.
                System.Threading.Tasks.Task tSaveMeeting = System.Threading.Tasks.Task.Run(async () =>
                {
                    await meeting.Save(SendInvitationsMode.SendToAllAndSaveCopy);
                });
                tSaveMeeting.Wait();

                // Verify that the meeting was created.
                Item item = null;
                System.Threading.Tasks.Task tItemBind = System.Threading.Tasks.Task.Run(async () =>
                {
                    item = await Item.Bind(Service, meeting.Id, new PropertySet(ItemSchema.Subject));
                });
                tItemBind.Wait();

                return new Response("Successfully created meeting", true);
            }
            catch (Exception ex)
            {
                return new Response($"Failed to create meeting\n{ex.Message}", false);
            }
        }

        public string ValidateRoomAddress(string roomAddress)
        {
            if (!roomAddress.EndsWith(addressDomain))
            {
                roomAddress += addressDomain;
            }

            return roomAddress;
        }

        private ExchangeService Service
        {
            get
            {
                var service = new ExchangeService(exchangeVersion)
                {
                    Credentials = new WebCredentials(username, password),
                    Url = new Uri(exchangeUrl)
                };
                return service;
            }
        }

        private void GetRoomAppointments(ref Room room, DateTime start, DateTime end)
        {
            List<AttendeeInfo> attend = new List<AttendeeInfo>();
            attend.Clear();
            attend.Add(room.Address);

            AvailabilityOptions options = new AvailabilityOptions
            {
                MaximumSuggestionsPerDay = 48,
            };

            GetUserAvailabilityResults userAvailability = null;
            System.Threading.Tasks.Task tUserAvailability = System.Threading.Tasks.Task.Run(async () =>
            {
                userAvailability = await Service.GetUserAvailability(attend, new TimeWindow(start, end), AvailabilityData.FreeBusyAndSuggestions, options);
            });
            tUserAvailability.Wait();

            foreach (AttendeeAvailability attendeeAvailability in userAvailability.AttendeesAvailability)
            {
                if (attendeeAvailability.ErrorCode == ServiceError.NoError)
                {
                    foreach (CalendarEvent calendarEvent in attendeeAvailability.CalendarEvents)
                    {
                        Event singleEvent = new Event
                        {
                            Start = calendarEvent.StartTime,
                            End = calendarEvent.EndTime,
                            Subject = calendarEvent.Details?.Subject,
                            Id = calendarEvent.Details?.StoreId
                        };
                        room.Events.Add(singleEvent);
                    }
                }
            }
        }

        private void GetRoomWeather(ref Room room)
        {
            SqlDataReader reader;
            var query = $"SELECT * FROM dbo.RoomWeather WHERE name='{room.Address}'";
            using (var conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    System.Diagnostics.Debug.WriteLine($"Getting room weather: {room.Address}");
                    conn.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        room.Temperature = double.Parse(reader["temperature"].ToString());
                        room.Humidity = double.Parse(reader["humidity"].ToString());
                    }
                    conn.Close();
                }
            }
        }

        private IEnumerable<Room> GetAllRooms(string roomFilter)
        {
            var roomsList = GetOrganizationRoomsList().Where(x => x.Name.Contains(roomFilter));
            var rooms = GetRoomsFromRoomsList(roomsList).Distinct().ToArray();
            for (int i = 0; i < rooms.Count(); i++)
            {
                GetRoomWeather(ref rooms[i]);
            }
            return rooms;
        }

        private EmailAddressCollection GetOrganizationRoomsList()
        {
            // Return all the room lists in the organization.
            EmailAddressCollection roomList = null;
            System.Threading.Tasks.Task tRoomList = System.Threading.Tasks.Task.Run(async () =>
            {
                roomList = await Service.GetRoomLists();
            });
            tRoomList.Wait();

            return roomList;
        }

        private IEnumerable<Room> GetRoomsFromRoomsList(IEnumerable<EmailAddress> roomsList)
        {
            // Collect all the rooms from every room list
            var rooms = new Collection<Room>();
            foreach (var room in roomsList)
            {
                System.Threading.Tasks.Task tRoomAddresses = System.Threading.Tasks.Task.Run(async () =>
                {
                    foreach (var item in await Service.GetRooms(room))
                    {
                        if (rooms.Any(s => s.Address == item.Address))
                        { continue; }

                        rooms.Add(new Room
                        {
                            Name = item.Name,
                            Address = item.Address
                        });
                    }
                });
                tRoomAddresses.Wait();
            }

            return rooms;
        }
    }
}