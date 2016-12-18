using System;
using System.Security.Cryptography;
using System.Text;

namespace Customers.Web.Helpers
{
    public static class HashedPasswordExtension
    {
        public static string GetMd5Hash(this string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static bool IsHashMatchesToPassword(this string hashedPassword, string providedPassword)
        {
            return string.Equals(hashedPassword, GetMd5Hash(providedPassword),
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}