using System.Security.Cryptography;
using System.Text;

namespace Transferciniz.API.Helpers;

public static class PasswordHelper
{
    public static string ToMD5(this string input)
    {
        // MD5CryptoServiceProvider sınıfını kullanarak MD5 hash'i hesaplıyoruz.
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Hash değerini hexadecimal formatta string'e çeviriyoruz.
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}