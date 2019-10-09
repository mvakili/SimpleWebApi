using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Model.Helpers
{
    public static class CryptoHelper
    {
        public static byte[] SHA1Hash(string data)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1data = sha1.ComputeHash(Encoding.ASCII.GetBytes(data));
            return sha1data;
        }
    }
}
