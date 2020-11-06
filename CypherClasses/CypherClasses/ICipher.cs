using System;
using System.Collections.Generic;
using System.Text;

namespace CypherClasses
{
    public interface ICipher<T>
    {
        bool Cipher(string route, out byte[] cipheredMsg, T Key);
        bool Decipher(string route, out byte[] Message, T Key);
    }
}
