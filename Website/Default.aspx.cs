///-----------------------------------------------------------------
/// <summary>
/// Default web page which displays the calendar events.
/// </summary>
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using DayPilot.Web.Ui.Enums.Calendar;
using RoomManager;

namespace Website
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly Exchange _exchange = new Exchange();
        private readonly DateTime _startDate = DateTime.Today;
        private readonly DateTime _endDate = DateTime.Today.AddDays(1);
        private IEnumerable<Room> _allRooms;
        private const int RoomCacheTimeMinutes = 1;
        private string _priorityRoomName = "POR-cr6@ptc.com";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initial call
                // Save off any data we don't want to re-acquire for this session
                _allRooms = _exchange.GetAllRoomsDetails();
                Session["AllRooms"] = _allRooms;
                PopulateLocationNames();
            }
            else
            {
                // Recall any data stored for this session
                _allRooms = (IEnumerable<Room>) Session["AllRooms"];
            }

            SetupCalendar();
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string roomArg = Request.QueryString["room"];
                var localLocation = string.Empty;
                var localRoom = string.Empty;
                if (roomArg == null)
                {
                    localLocation = _priorityRoomName.Split('-').FirstOrDefault();
                    localRoom = _allRooms.FirstOrDefault(s => s.Address.Contains(_priorityRoomName)).Address;
                }
                else
                {
                    localLocation = roomArg.Split('-').FirstOrDefault();
                    localRoom = _allRooms.FirstOrDefault(s => s.Address.Contains(roomArg)).Address;
                }

                var indexLocation = ddlLocations.Items.IndexOf(ddlLocations.Items.FindByText(localLocation));
                if (indexLocation > 0)
                {
                    ddlLocations.SelectedIndex = indexLocation;
                    DdlLocations_SelectedIndexChanged(this, e);
                }

                var indexRoom = ddlRooms.Items.IndexOf(ddlRooms.Items.FindByValue(localRoom));
                if (indexRoom > 0)
                {
                    ddlRooms.SelectedIndex = indexRoom;
                    DdlRooms_SelectedIndexChanged(this, e);
                }
            }
            else
            {
                string room = ddlRooms.SelectedItem.Value;
                Response.Redirect($"~/Default.aspx/?room={room}");
            }
        }

        private void SetupCalendar()
        {
            DayPilotCalendar1.ViewType = ViewTypeEnum.Day;
            DayPilotCalendar1.StartDate = _startDate;
            DayPilotCalendar1.DataStartField = "Start";
            DayPilotCalendar1.DataEndField = "End";
            DayPilotCalendar1.DataIdField = "Id";
            DayPilotCalendar1.DataTextField = "Subject";
            DayPilotCalendar1.HeaderDateFormat = "dddd MM/dd";
            DayPilotCalendar1.ShowEventStartEnd = true;
            DayPilotCalendar1.BusinessBeginsHour = 7;
            DayPilotCalendar1.BusinessEndsHour = 18;
            DayPilotCalendar1.HeightSpec = DayPilot.Web.Ui.Enums.HeightSpecEnum.BusinessHoursNoScroll;
        }

        private void UpdatePage(string roomName)
        {
            var room = GetRoom(roomName, _startDate, _endDate);

            lblLastUpdateTime.Text = $"Updated {room.LastUpdate.ToString("h:mm:ss tt")}";
            lblTemp.Text = $"{room.Temperature.ToString()}°F";
            lblHumidity.Text = $"{room.Humidity.ToString()}%";
            DayPilotCalendar1.DataSource = room.Events;
            DayPilotCalendar1.DataBind();
            DayPilotCalendar1.Update();
        }

        private Room GetRoom(string roomName, DateTime startDate, DateTime endDate)
        {
            // Data is only valid for a defined time span
            var requestedRoom = _allRooms.Single(s => s.Address.Contains(roomName));
            if (DateTime.Now.Subtract(requestedRoom.LastUpdate).TotalMinutes > RoomCacheTimeMinutes)
            {
                requestedRoom = _exchange.GetAppointmentsByRoomAddress(requestedRoom.Address, startDate, endDate);
                requestedRoom.LastUpdate = DateTime.Now;
            }

            return requestedRoom;
        }

        private void PopulateLocationNames()
        {
            var locations = new List<string>();
            foreach (var item in _allRooms)
            {
                var itemCollection = item.Name.Split('/', '\\');
                var location = itemCollection.FirstOrDefault();
                if (!locations.Contains(location))
                    locations.Add(location);
            }

            ddlLocations.DataSource = locations;
            ddlLocations.DataBind();
            ddlLocations.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        private void PopulateRoomNames()
        {
            var roomNames = new List<ListItem>();
            foreach (var item in _allRooms)
            {
                var itemCollection = item.Name.Split('/', '\\');
                var room = itemCollection.Skip(1).FirstOrDefault();
                roomNames.Add(new ListItem(room, item.Address));
            }

            var localRooms = roomNames.Where(s => s.Value.Contains(ddlLocations.SelectedItem.Text));
            ddlRooms.DataSource = localRooms;
            ddlRooms.DataTextField = "Text";
            ddlRooms.DataValueField = "Value";
            ddlRooms.DataBind();
            ddlRooms.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void DdlLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateRoomNames();
            DayPilotCalendar1.DataSource = new List<ListItem>();
            DayPilotCalendar1.DataBind();
        }

        protected void DdlRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePage(ddlRooms.SelectedItem.Value);
        }
    }
}