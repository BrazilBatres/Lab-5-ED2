using System;
using CypherClasses;
namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Escriba la clave: ");
            //string key = Console.ReadLine();
            int key = int.Parse(Console.ReadLine());
            Console.WriteLine("Mensaje a cifrar: ");
            string msg = Console.ReadLine();
            byte[] mesg = new byte[msg.Length];
            for (int i = 0; i < msg.Length; i++)
            {
                mesg[i] = (byte)msg[i];
            }
            César césar = new César();
            //césar.SetKey(key);
            ZigZag zigZag = new ZigZag();
            zigZag.SetLevels(key);
            bool success = zigZag.Cipher(mesg, out byte[] encr);
            //Sár eimiadsmin  asuaiamt m ,gdmnao e  ds
            //zigZag.SetFillChar(0);
            //bool success = zigZag.Decipher(mesg, out byte[] encr);
            if (success)
            {
                foreach (var item in encr)
                {
                    Console.Write(((char)item).ToString());
                }
            }
            else
            {
                Console.WriteLine("fail");
            }
            Console.ReadKey();
        }
    }
}
