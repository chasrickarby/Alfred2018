using System;
using System.Collections.Generic;
using LibExchange;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RestServer.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : Controller
    {
        private Exchange exchange = new Exchange();

        // GET api/rooms
        [HttpGet]
        public string Get()
        {
            IEnumerable<Room> roomDetails = exchange.GetAllRoomsDetails();
            string json = JsonConvert.SerializeObject(roomDetails);
            return json;
        }

        // GET api/rooms/POR-cr6@ptc.com
        [HttpGet("{address}")]
        public string Get(string address)
        {
            Room room = exchange.GetAppointmentsByRoomAddress(address, DateTime.Today, DateTime.Today.AddDays(1));
            string json = JsonConvert.SerializeObject(room);
            return json;
        }
    }
}
