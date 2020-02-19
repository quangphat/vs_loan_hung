using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Utility
{
    public static class Utility
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string HmacSha256(string originalData, string secretKey)
        {
            var hashed = string.Empty;

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                hashed = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(originalData)));

            return hashed;
        }
       
    }
    
}
