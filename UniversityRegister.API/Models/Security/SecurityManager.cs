using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace UniversityRegister.API.Models.Security
{
    public static class SecurityManager
    {
        public static byte[] GenerateSalt(int length)
        {
            var bytes = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return bytes;
        }

        public static string GetHash(string password, byte[] salt, int iterations)
        {
            string hash = String.Empty;

            try
            {
                using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    var bytes = deriveBytes.GetBytes(salt.Length);
                    hash = Convert.ToBase64String(bytes);
                }
            }
            catch (Exception e)
            {

                throw;
            }


            return hash;
        }
    }
}
