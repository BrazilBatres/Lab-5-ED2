using System;
using System.Collections.Generic;
using System.Text;

namespace CypherClasses
{
    public interface ICipher
    {
        bool Cipher(string route, out byte[] cipheredMsg);
        bool Decipher(string route, out byte[] Message);
    }
}
