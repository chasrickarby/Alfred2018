///-----------------------------------------------------------------
/// <summary>
/// Communication with SunFounder temperature/humidy sensor.
/// </summary>
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.Gpio;

namespace Sensors
{
    public class TempHumiditySensor : ITempHumiditySensor
    {
        private static Gpio gpio;

        public TempHumiditySensor()
        {
            gpio = new Gpio();
        }

        public TempHumidityData GetData()
        {
            // TODO: Fake News
            var data = new TempHumidityData
            {
                Humidity = 40,
                TempCelsius = 20
            };

            return data;
        }
    }
}