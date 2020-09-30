using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AppUtility.AppEncription
{
    public static class CommonEncription
    {
        public static string ConvertObjectToBase64String<T>(T obj)
        {            
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
            return System.Convert.ToBase64String(plainTextBytes);            
        }

        public static T ConvertBase64StringToObject<T>(string base64String)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64String);
            return JsonSerializer.Deserialize<T>(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
        }
        
    }
}
