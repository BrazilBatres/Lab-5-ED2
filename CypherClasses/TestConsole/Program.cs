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
            //    Console.WriteLine("Escriba la clave: ");
            //    //string key = Console.ReadLine();
            //    int key = int.Parse(Console.ReadLine());
            //    Console.WriteLine("Mensaje a cifrar: ");
            //    string msg = Console.ReadLine();
            //    byte[] mesg = new byte[msg.Length];
            //    for (int i = 0; i < msg.Length; i++)
            //    {
            //        mesg[i] = (byte)msg[i];
            //    }
            //    César césar = new César();
            //    //césar.SetKey(key);
            //    ZigZag zigZag = new ZigZag();
            //    zigZag.SetLevels(key);
            //    bool success = zigZag.Cipher(mesg, out byte[] encr);
            //    //Sár eimiadsmin  asuaiamt m ,gdmnao e  ds
            //    //zigZag.SetFillChar(0);
            //    //bool success = zigZag.Decipher(mesg, out byte[] encr);
            //    if (success)
            //    {
            //        foreach (var item in encr)
            //        {
            //            Console.Write(((char)item).ToString());
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("fail");
            //    }
            //    Console.ReadKey();
            string uploadPath = @"C:\Users\brazi\Desktop\Pruebas Cifrado2.txt";
            byte[] FileBytes;
            //Ruta ruta = new Ruta();
            //César césar = new César();
            ZigZag césar = new ZigZag();
            césar.Decipher(uploadPath, out FileBytes, 4);
            using (FileStream fs = File.Create(@"C:\Users\brazi\Desktop\FuncionaPorfi.txt"))
            {
                fs.Write(FileBytes);
            }

            //using (FileStream fs = File.OpenRead(@"C:\Users\joseg\Desktop\Pruebas Cifrado\Originales\cuento.txt"))
            //{
            //    using (FileStream fs2 = File.OpenRead(@"C:\Users\joseg\Desktop\Pruebas Cifrado\Descifrados\cuento2.txt"))
            //    {
            //        using (BinaryReader reader = new BinaryReader(fs))
            //        {
            //            using (BinaryReader reader1 = new BinaryReader(fs2))
            //            {
            //                int counter = 0;
            //                byte array1;
            //                byte array2;
            //                while (counter<fs.Length)
            //                {
            //                    array1 = reader.ReadByte();
            //                    array2 = reader1.ReadByte();
            //                    counter ++;
            //                    if (array2 != array1)
            //                    {
            //                        Console.WriteLine(false);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}
