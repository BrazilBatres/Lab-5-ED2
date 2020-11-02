using System;
using System.Collections.Generic;
using System.Text;

namespace CypherClasses
{
    public interface ICipher
    {
        bool Cipher(byte[] message, out byte[] cipheredMsg);
        bool Decipher(byte[] EncryptedMessage, out byte[]Message);
    }
}
