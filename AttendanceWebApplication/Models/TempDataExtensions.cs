using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace AttendanceWebApplication.Models
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key)
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? default(T) : JsonSerializer.Deserialize<T>((string)o);
        }
    }

}
