using System;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Schemas;

namespace Sensors.Rest
    {
    [RestController(InstanceCreationType.Singleton)]
    public class SensorController
        {
        private string foo;

        public SensorController(string foo)
            {
            this.foo = foo;
            }

        [UriFormat("/data")]
        public GetResponse GetData()
            {
            var sensor = new TempHumiditySensor();
            return new GetResponse(GetResponse.ResponseStatus.OK,
                sensor.GetData());
            }
        }
    }