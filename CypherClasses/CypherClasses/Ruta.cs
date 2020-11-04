using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CypherClasses
{
    public class Ruta : ICipher
    {
        List<byte[,]> Matrixes = new List<byte[,]>();
        int columns = 0;
        int rows = 0;
        public void SetSize(int Rows, int Columns)
        {
            rows = Rows;
            columns = Columns;
        }
        public bool Cipher(string route, out byte[] cipheredMsg)
        {
            using (FileStream fs = File.OpenRead(route))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    FillMatrixes(reader.ReadBytes(Convert.ToInt32(fs.Length)));
                    cipheredMsg = VerticalArrangement();
                    return true;
                }
            }
        }

        public void FillMatrixes(byte[] message)
        {
            int index = 0;
            while (index < message.Length)
            {
                byte[,] aux = new byte[rows, columns];
                for (int i = 0; i < rows; i++)
                {
                    for (int k = 0; k < columns; k++)
                    {
                        if (index < message.Length)
                        {
                            aux[i, k] = message[index];
                            index++;
                        }
                        else
                        {
                            aux[i, k] = 170;
                        }
                    }
                }
                Matrixes.Add(aux);
            }
        }
        public byte[] SpiralArrangement()
        {
            return null;
        }

        public byte[] VerticalArrangement()
        {
            List<byte> toReturn = new List<byte>();
            while (Matrixes.Count != 0)
            {
                byte[,] aux = Matrixes.First();
                Matrixes.Remove(Matrixes.First());
                for (int i = 0; i < rows; i++)
                {
                    for (int k = 0; k < columns; k++)
                    {
                        if (aux[k,i] != 170)
                        {
                            toReturn.Add(aux[k, i]);
                        }
                    }
                }
            }
            return toReturn.ToArray();
        }
        public bool Decipher(byte[] EncryptedMessage, out byte[] Message)
        {
            Message = null;
            return false;
        }
    }
}
