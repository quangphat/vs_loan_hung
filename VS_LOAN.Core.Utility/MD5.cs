using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VS_LOAN.Core.Utility
{
    public static class MD5
    {
        static string start = "KPMG_EV";
        static string end = "KPMG_PM";
        private static byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public static string getMD5(string data)
        {
            return BitConverter.ToString(encryptData(start + data + end)).Replace("-", "").ToLower();
        }
    }
}
