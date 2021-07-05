using System.Security.Cryptography;
using System.Text;

namespace Sushi.Room.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string ToSha256(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return default;
            }
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("x2"));
            }
            return result.ToString();
        }
    }
}
