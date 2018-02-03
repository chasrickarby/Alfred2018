///-----------------------------------------------------------------
/// <summary>
/// Communication with Microsoft EWS (Exchange Web Service).
/// </summary>
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Exchange.WebServices.Data;

namespace LibExchange
{
    public class Exchange : IExchange
    {
        private readonly string username = "user@ptc.com";
        private readonly string password = "password";
        private readonly string exchangeUrl = "https://outlook.office365.com/ews/exchange.asmx";
        private readonly ExchangeVersion exchangeVersion = ExchangeVersion.Exchange2010;
        private const string roomFilter = "POR/";

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
            }

            return rooms;
        }

        public Room GetAppointmentsByRoomAddress(string roomAddress, DateTime start, DateTime end)
        {
            var room = new Room
            {
                Address = roomAddress
            };
            GetRoomAppointments(ref room, start, end);
            return room;
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

        private IEnumerable<Room> GetAllRooms(string roomFilter)
        {
            var roomsList = GetOrganizationRoomsList().Where(x => x.Name.Contains(roomFilter));
            return GetRoomsFromRoomsList(roomsList).Distinct();
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
                        if(rooms.Any(s => s.Address == item.Address))
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