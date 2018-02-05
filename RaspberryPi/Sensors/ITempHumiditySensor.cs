///-----------------------------------------------------------------
/// <summary>
/// Operations available on temperature/humidity sensor.
/// </summary>
///-----------------------------------------------------------------

namespace Sensors
{
    public interface ITempHumiditySensor
    {
        TempHumidityData GetData();
    }
}