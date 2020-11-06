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
                    FillSpiral(reader.ReadBytes(Convert.ToInt32(fs.Length)));
                    cipheredMsg = HorizontalArrangement();
                    return true;
                }
            }
        }
        public byte[] HorizontalArrangement()
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
                        if (aux[i, k] != 170)
                        {
                            toReturn.Add(aux[i, k]);
                        }
                    }
                }
            }
            return toReturn.ToArray();
        }
        public void FillSpiral(byte[] message)
        {
            int index = 0;
            while (index < message.Length)
            {
                byte[,] aux = new byte[rows, columns];
                int n = columns;
                int m = rows;
                int i, k = 0, l = 0;
                while (k < m && l < n)
                {
                    for (i = l; i < n; ++i)
                    {
                        if (index < message.Length)
                        {
                            aux[k, i] = message[index];
                            index++;
                        }
                        else
                        {
                            aux[k, i] = 170;
                        }
                    }
                    k++;
                    for (i = k; i < m; ++i)
                    {
                        if (index < message.Length)
                        {
                            aux[i, n - 1] = message[index];
                            index++;
                        }
                        else
                        {
                            aux[i, n - 1] = 170;
                        }
                    }
                    n--;
                    if (k < m)
                    {
                        for (i = n - 1; i >= l; --i)
                        {
                            if (index < message.Length)
                            {
                                aux[m - 1, i] = message[index];
                                index++;
                            }
                            else
                            {
                                aux[m - 1, i] = 170;
                            }
                        }
                        m--;
                    }
                    if (l < n)
                    {
                        for (i = m - 1; i >= k; --i)
                        {
                            if (index < message.Length)
                            {
                                aux[i, l] = message[index];
                                index++;
                            }
                            else
                            {
                                aux[i, l] = 170;
                            }
                        }
                        l++;
                    }
                }
                Matrixes.Add(aux);
            }
        }
        public bool Decipher(string route, out byte[] Message)
        {
            using (FileStream fs = File.OpenRead(route))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    FillHorizontal(reader.ReadBytes(Convert.ToInt32(fs.Length)));
                    Message = SpiralArrangement();
                    return true;
                }
            }
        }

        public void FillHorizontal(byte[] message)
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
            List<byte> toReturn = new List<byte>();
            while (Matrixes.Count != 0)
            {
                int i = 0;
                int m = rows;
                int l = 0;
                int n = columns;
                int k = 0;

                byte[,] aux = Matrixes.First();
                Matrixes.Remove(Matrixes.First());
                while (k < m && l < n)
                {
                    
                    for (i = l; i < n; ++i)
                    {
                        if (aux[k,i] != 170)
                        {
                            toReturn.Add(aux[k, i]);
                        }
                    }
                    k++;

                    for (i = k; i < m; ++i)
                    {
                        if (aux[i, n-1] != 170)
                        {
                            toReturn.Add(aux[i, n - 1]);
                        }
                    }
                    n--;

                    if (k < m)
                    {
                        for (i = n - 1; i >= l; --i)
                        {
                            if (aux[m-1,i] != 170)
                            {
                                toReturn.Add(aux[m - 1, i]);
                            }
                        }
                        m--;
                    }

                    if (l < n)
                    {
                        for (i = m - 1; i >= k; --i)
                        {
                            if (aux[i,l] != 170)
                            {
                                toReturn.Add(aux[i, l]);
                            }
                        }
                        l++;
                    }
                }

            }
            return toReturn.ToArray();
        }

    }
}
