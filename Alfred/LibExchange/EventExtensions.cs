///-----------------------------------------------------------------
/// <summary>
/// Extensions for converting data.
/// </summary>
///-----------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace LibExchange
{
    public static class EventExtensions
    {
        public static DataTable ConvertToDataTable(this List<Event> events)
        {
            DataTable table = new DataTable();
            PropertyInfo[] eventProperties = typeof(Event).GetProperties();

            foreach (PropertyInfo eventProperty in eventProperties)
            {
                table.Columns.Add(eventProperty.Name);
            }

            foreach (var item in events)
            {
                var rowValues = new List<string>();
                PropertyInfo[] itemProperties = item.GetType().GetProperties();
                foreach(var value in itemProperties)
                {
                    rowValues.Add(value.GetValue(item).ToString());
                }

                table.Rows.Add(rowValues.ToArray());
            }

            return table;
        }
    }
}