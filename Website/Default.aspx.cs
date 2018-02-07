///-----------------------------------------------------------------
/// <summary>
/// Default web page which displays the calendar events.
/// </summary>
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using DayPilot.Web.Ui.Enums.Calendar;
using RoomManager;

namespace Website
{
    public partial class Default : System.Web.UI.Page
    {
        Exchange exchange = new Exchange();
        DateTime startDate = DateTime.Today;
        DateTime endDate = DateTime.Today.AddDays(1);
        IEnumerable<Room> allRooms;
        int roomCacheTimeMinutes = 5;
        string priorityRoomName = "POR/ESC";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initial call
                // Save off any data we don't want to re-acquire for this session
                allRooms = exchange.GetAllRoomsDetails();
                Session["AllRooms"] = allRooms;

                var roomNames = allRooms.Select(x => x.Name).Distinct();
                ddlRooms.DataSource = roomNames;
                ddlRooms.DataBind();
            }
            else
            {
                // Recall any data stored for this session
                allRooms = (IEnumerable<Room>)Session["AllRooms"];
            }
                        
            SetupCalendar();
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var index = ddlRooms.Items.IndexOf(ddlRooms.Items.FindByText(priorityRoomName));
                if (index > 0)
                {
                    ddlRooms.SelectedIndex = index;
                }

                DdlRooms_SelectedIndexChanged(ddlRooms, e);
            }
        }

        private void SetupCalendar()
        {
            DayPilotCalendar1.ViewType = ViewTypeEnum.Day;
            DayPilotCalendar1.StartDate = startDate;
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
            var room = GetRoom(roomName, startDate, endDate);

            lblLastUpdateTime.Text = $"Updated {room.LastUpdate.ToString("h:mm:ss tt")}";

            DayPilotCalendar1.DataSource = room.Events;
            DayPilotCalendar1.DataBind();
            DayPilotCalendar1.Update();
        }

        private Room GetRoom(string roomName, DateTime startDate, DateTime endDate)
        {
            // Data is only valid for a defined time span
            var requestedRoom = allRooms.Single(s => s.Name == roomName);
            if (DateTime.Now.Subtract(requestedRoom.LastUpdate).TotalMinutes > roomCacheTimeMinutes)
            {
                var appointments = exchange.GetAppointmentsByRoomAddress(requestedRoom.Address, startDate, endDate).Events;
                requestedRoom.Events = appointments;
                requestedRoom.LastUpdate = DateTime.Now;
            }

            return requestedRoom;
        }

        protected void DdlRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePage(((System.Web.UI.WebControls.ListControl)sender).SelectedValue);
        }
    }
}