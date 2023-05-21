using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniChatApplication.Daos
{
    public class CookieDaos
    {
        /// <summary>
        /// Create Cookie Login
        /// </summary>
        /// <returns>String with 24 random characters</returns>
        public static string CreateCookieLogin()
        {
            int length = 24;
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();


            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (
                var rng =
                    System.Security.Cryptography.RandomNumberGenerator.Create()
            )
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (
                        var i = 0;
                        i < buf.Length && result.Length < length;
                        ++i
                    )
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart =
                            byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result
                            .Append(allowedCharSet[buf[i] %
                            allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
    }
}
