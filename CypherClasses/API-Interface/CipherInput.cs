using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CypherClasses;
using Microsoft.AspNetCore.Http;

namespace API_Interface
{
    public class CipherInput
    {
        public IFormFile File { get; set; }
        public CipherKey Key { get; set; }
    }
}
