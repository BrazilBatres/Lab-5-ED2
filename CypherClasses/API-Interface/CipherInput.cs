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
        public string Word { get; set; }
        public int Levels { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}
