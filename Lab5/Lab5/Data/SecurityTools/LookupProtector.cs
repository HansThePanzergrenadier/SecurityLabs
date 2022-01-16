using Microsoft.AspNetCore.Identity;
using System;


namespace Lab5.Data.SecurityTools
{
    public class LookupProtector : ILookupProtector
    {
        private readonly ILookupProtectorKeyRing _keyRing;

        public LookupProtector(ILookupProtectorKeyRing keyRing)
        {
            _keyRing = keyRing;
        }

        public string Protect(string keyId, string data)
        {
            throw new NotImplementedException();
        }

        public string Unprotect(string keyId, string data)
        {
            throw new NotImplementedException();
        }
    }
}
