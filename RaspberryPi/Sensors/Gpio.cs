using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.Gpio;

namespace Sensors
    {
    public class Gpio
        {
        private GpioController gpio;
        private List<int> gpioIds = new List<int>() { 9, 8, 7, 6, 5, 4, 3, 2 };
        private List<GpioPin> gpioPins = new List<GpioPin>();
        private bool initialized = false;

        public void SetPins(int value)
            {
            var binaryValue = ConvertDecimalToBinaryArray(value);
            Debug.WriteLine($"Setting lights: {string.Join("", binaryValue.Reverse())}");

            Initialize();
            for (int i = 0; i< binaryValue.Length; i++)
                {
                var bin = binaryValue[i];
                switch (bin)
                    {
                    case 0:
                        // Turn OFF output
                        gpioPins[i].Write(GpioPinValue.Low);
                        break;
                    case 1:
                        // Turn ON output
                        gpioPins[i].Write(GpioPinValue.High);
                        break;
                    default:
                        Debug.WriteLine($"Unknown binary value: {bin}");
                        break;
                    }
                }
            }

        private void Initialize()
            {
            if (initialized)
                {
                return;
                }

            initialized = true;

            Debug.WriteLine("Initializing GPIO");
            gpio = GpioController.GetDefault();

            // GPIO 02 = PIN 03
            // GPIO 03 = PIN 05
            // GPIO 04 = PIN 07
            // GPIO 05 = PIN 29
            // GPIO 06 = PIN 31
            // GPIO 07 = PIN 26
            // GPIO 08 = PIN 24
            // GPIO 09 = PIN 21

            foreach (var gpioId in gpioIds)
                {
                Debug.WriteLine($"Initializing GPIO pin: {gpioId}");
                var gpioPin = gpio.OpenPin(gpioId);

                // Initialize pin and push the pin 'low' to turn it off
                gpioPin.SetDriveMode(GpioPinDriveMode.Output);
                gpioPin.Write(GpioPinValue.Low);

                gpioPins.Add(gpioPin);
                }
            }

        private int[] ConvertDecimalToBinaryArray(int value)
            {
            // DEC BINARY
            // 1 = 0000 0001
            // 2 = 0000 0010
            // 4 = 0000 0100
            // 8 = 0000 1000
            // 16 = 0001 0000
            // 32 = 0010 0000
            // 64 = 0100 0000
            // 128 = 1000 0000

            if (value > 225)
                {
                value = 255;
                }

            BitArray bitarray = new BitArray(new byte[] { Convert.ToByte(value) });
            return bitarray.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray();
            }
        
        }
    }
