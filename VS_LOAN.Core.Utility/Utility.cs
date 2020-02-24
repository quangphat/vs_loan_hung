using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
        public static bool IsValidEmail(string email, int maxLength = 255)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            if (email.Length > maxLength)
            {
                return false;
            }

            var patternEmail =
            "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

            return Regex.IsMatch(email, patternEmail);
        }
        public static bool IsValidIdentityCard(string identityNumber)
        {
            if (string.IsNullOrWhiteSpace(identityNumber))
                return false;
            var pattern = "^(\\d{9}|\\d{12})$";
            return Regex.IsMatch(identityNumber, pattern);
        }
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;
            var pattern = "^0(\\d{9,10})$";
            return Regex.IsMatch(phone, pattern);
        }
    }
    
}
