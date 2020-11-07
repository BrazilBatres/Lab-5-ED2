using System;
using System.IO;
using System.Linq;
using CypherClasses;
namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream fs = File.OpenRead(@"C:\Users\joseg\Desktop\Pruebas Cifrado\Originales\cuento.txt"))
            {
                using (FileStream fs2 = File.OpenRead(@"C:\Users\joseg\Desktop\Pruebas Cifrado\Descifrados\ZigZag\cuentozigzag.txt"))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        using (BinaryReader reader1 = new BinaryReader(fs2))
                        {
                            int counter = 0;
                            byte array1;
                            byte array2;
                            while (counter < fs.Length)
                            {
                                array1 = reader.ReadByte();
                                array2 = reader1.ReadByte();
                                counter++;
                                if (array2 != array1)
                                {
                                    Console.WriteLine(false);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
