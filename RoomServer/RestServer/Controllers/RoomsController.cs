using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoomManager;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace RestServer.Controllers
{
    public class RoomsController : ApiController
    {
        static readonly IExchange exchange = new Exchange();

        // GET api/rooms/
        public IEnumerable<Room> GetAllRoomDetails()
        {
            IEnumerable<Room> roomDetails = exchange.GetAllRoomsDetails();
            return roomDetails;
        }

        // GET /RestServer/api/rooms?id=POR-cr6@ptc.com
        // GET /RestServer/api/rooms/POR-cr6
        public Room GetAppointmentsByRoomAddress(string id)
        {
            Room room = exchange.GetAppointmentsByRoomAddress(id, DateTime.Today, DateTime.Today.AddDays(1));
            return room;
        }

        // POST /RestServer/api/rooms/CreateMeeting/?roomAddress=POR-cr6&subject=blah blah&start=2018-02-06T00:00:00&end=2018-02-07T00:00:00
        public HttpResponseMessage CreateMeeting(string id, string subject, DateTime start, DateTime end)
        {
            var meetingRequestResponse = exchange.SendMeetingRequest(id, subject, start, end);

            HttpResponseMessage response;
            if (meetingRequestResponse.Success)
            {
                response = Request.CreateResponse(HttpStatusCode.Created, meetingRequestResponse.Message);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.Created, meetingRequestResponse.Message);
            }

            string uri = Url.Link("DefaultApi", new { id, subject, start, end });
            response.Headers.Location = new Uri(uri);
            return response;
        }
    }
}
