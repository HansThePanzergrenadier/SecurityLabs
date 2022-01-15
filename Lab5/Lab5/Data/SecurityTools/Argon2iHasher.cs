using Microsoft.AspNetCore.Identity;
using Sodium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5.Data.SecurityTools
{
    public class Argon2iHasher : IPasswordHasher<IdentityUser>
    {
        public string HashPassword(IdentityUser user, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            return PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Interactive).TrimEnd('\0');
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentNullException(nameof(hashedPassword));
            if (string.IsNullOrWhiteSpace(providedPassword)) throw new ArgumentNullException(nameof(providedPassword));

            var isValid = PasswordHash.ArgonHashStringVerify(hashedPassword, providedPassword);

            if (isValid && PasswordHash.ArgonPasswordNeedsRehash(hashedPassword, PasswordHash.StrengthArgon.Interactive))
            {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }

            return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
