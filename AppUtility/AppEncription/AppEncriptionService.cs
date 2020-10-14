using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AppUtility.AppEncription
{    
    public interface IEncriptionService
    {
        string ConvertObjectToBase64String<T>(T obj);
        T ConvertBase64StringToObject<T>(string base64String);
        string EncriptWithPrivateKey(string StrValue);
        string DecriptWithPrivateKey(string StrValue);
    }

    public class EncriptionService : IEncriptionService
    {       
        public EncriptionService()
        {           
        }

        public string ConvertObjectToBase64String<T>(T obj)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public T ConvertBase64StringToObject<T>(string base64String)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64String);
            return JsonSerializer.Deserialize<T>(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
        }

        public string EncriptWithPrivateKey(string StrValue)
        {
            // write encription code
            return StrValue;
        }

        public string DecriptWithPrivateKey(string StrValue)
        {
            // write decription code
            return StrValue;
        }
    }
}
