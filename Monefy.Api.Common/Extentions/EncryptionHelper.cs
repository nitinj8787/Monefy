using System;
using System.Collections.Generic;
using System.Text;

namespace Monefy.Api.Common.Extentions
{
    using BCrypt = BCrypt.Net.BCrypt;
    public static class EncryptionHelper
    {
        public static string WithBCrypt(this string text)
        {
            return BCrypt.HashPassword(text);
        }

        public static bool VerifyWithBCrypt(this string hashedPassword, string textPassword)
        {
            return BCrypt.Verify(textPassword, hashedPassword);
        }
    }
}
