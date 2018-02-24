

using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.HelperClasses
{
    /// <summary>
    /// For security reasons passwords should be saved as hashes to the database
    /// For increased security, salt should be used along with the password
    /// Even if the salt is known, it is much harder to bruteforce the password
    /// Also, 2 same passwords with different salt will be hashed differently
    /// </summary>
    public static class HashedPasswordWithSalt
    {
        // getSalt
        public static string getSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[32];

            rng.GetBytes(buffer);
            return BitConverter.ToString(buffer).Replace("-", "").ToLower();

            //return BitConverter.ToString(saltArray).Replace("-", "");
        }

        // getHash
        public static string getHash(string password, string salt)
        {
            byte[] hash;
            string passwordToHashWithSalt = password + salt;
            using (SHA512 shaM = new SHA512Managed())
            {                
                hash = shaM.ComputeHash(Encoding.UTF8.GetBytes(passwordToHashWithSalt));
            }

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }


    }
}
