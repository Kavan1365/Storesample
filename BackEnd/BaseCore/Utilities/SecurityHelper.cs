using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace BaseCore.Utilities
{
    public class PasswordHasher
    {
        const int IterCount = 1000;
        const int SubkeyLength = 32;
        const int SaltSize = 16;

        public static string HashPasswordV2(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            byte[] saltBytes;
            byte[] hashedPasswordBytes;
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSize, IterCount))
            {
                saltBytes = rfc2898DeriveBytes.Salt;
                hashedPasswordBytes = rfc2898DeriveBytes.GetBytes(SubkeyLength);
            }
            var outArray = new byte[1 + SaltSize + SubkeyLength];
            outArray[0] = 0x00;
            Buffer.BlockCopy(saltBytes, 0, outArray, 1, SaltSize);
            Buffer.BlockCopy(hashedPasswordBytes, 0, outArray, 1 + SaltSize, SubkeyLength);
            return Convert.ToBase64String(outArray);
        }

        public static bool VerifyHashedPasswordV2(string hashedPassword, string password)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            if (hashedPasswordBytes.Length != 1 + SaltSize + SubkeyLength)
            {
                return false; // bad size
            }

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, salt.Length);

            byte[] expectedSubkey = new byte[SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);


            byte[] passwordBytes;
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, IterCount))
            {
                passwordBytes = rfc2898DeriveBytes.GetBytes(SubkeyLength);
            }

            return ByteArraysEqual(passwordBytes, expectedSubkey);
        }


        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

    }
    public static class Helper
    {
        public static string MD5Hash(this string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }

}
