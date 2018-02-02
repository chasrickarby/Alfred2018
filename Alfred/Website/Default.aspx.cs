///-----------------------------------------------------------------
/// <summary>
/// Default web page which displays the calendar events.
/// </summary>
///-----------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Enums.Calendar;
using DayPilot.Web.Ui.Events;
using LibExchange;

namespace Website
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) => LoadAppointments();

        private void LoadAppointments()
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(1);
            var exchange = new Exchange();
            var allRooms = exchange.GetAllRoomsDetails();

            DayPilotCalendar1.ViewType = ViewTypeEnum.Day;
            DayPilotCalendar1.StartDate = startDate;
            DayPilotCalendar1.DataStartField = "Start";
            DayPilotCalendar1.DataEndField = "End";
            DayPilotCalendar1.DataIdField = "Id";
            DayPilotCalendar1.DataTextField = "Subject";
            DayPilotCalendar1.HeaderDateFormat = "dddd MM/dd";
            DayPilotCalendar1.ShowEventStartEnd = true;
            DayPilotCalendar1.BusinessBeginsHour = 7;
            DayPilotCalendar1.BusinessEndsHour = 17;
            DayPilotCalendar1.HeightSpec = DayPilot.Web.Ui.Enums.HeightSpecEnum.BusinessHours;

            // Find room name ESC from list of all rooms
            var roomName = allRooms.Single(x => x.Name.Contains("ESC")).Address;
            DayPilotCalendar1.DataSource = exchange.GetAppointmentsByRoomAddress(roomName, startDate, endDate).Events;

            DayPilotCalendar1.DataBind();
            DayPilotCalendar1.Update();
        }
    }
}