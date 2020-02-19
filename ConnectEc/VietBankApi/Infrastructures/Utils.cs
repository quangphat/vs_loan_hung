using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VietBankApi.Infrastructures
{
    public static class Utils
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string AddQuery(this string path, object obj)
        {
            if (path == null || obj == null)
                return path;

            return QueryHelpers.AddQueryString(path, obj.ToKeyPairs().ToDictionary(m => m.Key, m => m.Value.ToString()));
        }
        public static IEnumerable<KeyValuePair<string, object>> ToKeyPairs(this object obj)
        {
            if (obj == null)
                yield break;

            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj);
                if (value != null)
                    yield return new KeyValuePair<string, object>(property.Name, value);
            }
        }
        public static string HmacSha256(string originalData, string secretKey)
        {
            var hashed = string.Empty;

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                hashed = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(originalData)));

            return hashed;
        }
        public static string ToJson<T>(this T obj)
        {
            if (obj == null)
                return "object null";
            var result = JsonConvert.SerializeObject(obj);
            return result;
        }
    }
}
