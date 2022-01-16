using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;


namespace Lab5.Data.SecurityTools
{
    public class KeyRing : ILookupProtectorKeyRing
    {
        public KeyRing(IWebHostEnvironment hostingEnvironment)
        {
            
        }

        public string this[string keyId] => throw new NotImplementedException();

        public string CurrentKeyId => throw new NotImplementedException();

        public IEnumerable<string> GetAllKeyIds()
        {
            throw new NotImplementedException();
        }
    }
}
