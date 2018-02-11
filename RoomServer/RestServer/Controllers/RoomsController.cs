using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoomManager;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Http.Cors;

namespace RestServer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RoomsController : ApiController
    {
        private static readonly IExchange exchange = new Exchange();
        private static System.Data.SqlClient.SqlConnection conn;

        public RoomsController()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
            conn = new SqlConnection
            {
                ConnectionString = connectionString
            };
        }

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

            string uri = Url.Link("DefaultApi", new {id, subject, start, end});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // POST /RestServer/api/rooms/UpdateRoomWeather/?roomAddress=POR-cr6&temperature=70&humidity=40
        public HttpResponseMessage UpdateRoomWeather(string roomAddress, string temperature, string humidity)
        {
            var fullAddress = exchange.ValidateRoomAddress(roomAddress);
            HttpResponseMessage response;
            try
            {
                string query = string.Empty;
                if (ExistsInDatabase(fullAddress))
                {
                    query = "UPDATE dbo.RoomWeather " +
                            "SET temperature=@Temperature, humidity=@Humidity " +
                            "WHERE name=@Name";
                }
                else
                {
                    query = "INSERT INTO dbo.RoomWeather (Name, Temperature, Humidity) " +
                            "VALUES (@Name, @Temperature, @Humidity);";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", fullAddress);
                    cmd.Parameters.AddWithValue("@Temperature", double.Parse(temperature));
                    cmd.Parameters.AddWithValue("@Humidity", double.Parse(humidity));
                    int result = cmd.ExecuteNonQuery();
                    response = Request.CreateResponse(HttpStatusCode.Created, result);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        private bool ExistsInDatabase(string roomAddress)
        {
            var query = "SELECT COUNT(*) FROM dbo.RoomWeather WHERE [name]=@roomaddress";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@roomaddress", roomAddress);
                return (int) cmd.ExecuteScalar() == 1;
            }
        }
    }
}