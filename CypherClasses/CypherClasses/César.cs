using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CypherClasses
{
    public class César : ICipher<string>
    {
        Dictionary<char, char> LetterPairs = new Dictionary<char, char>();
        //bool invalidKey = true;
        string Key = "";
        
        public bool SetKey(string key)
        {
            Key = key.ToUpper();
            
            bool invalidKey = false;
            List<char> auxList = new List<char>();
            
            for (int i = 0; i < Key.Length; i++)
            {
                if (!(Key[i] > 64 && Key[i] < 91 && !auxList.Contains(Key[i])))
                {
                    
                    invalidKey = true;
                    
                    break;
                }
                
            }
            return !invalidKey;
        }
        public bool Cipher(string route, out byte[] CipheredMsg, string _key)
        {
            CipheredMsg = null;
            byte[] message;
            bool validKey = SetKey(_key);
            if (validKey)
            {
                using (FileStream fs = File.OpenRead(route))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        int counter = 0;
                        FillDictionary(true);
                        while (counter<fs.Length)
                        {
                            message = reader.ReadBytes(1000);
                            
                            CipheredMsg = ReplaceChars(message);
                            counter += 100;
                        }
                        
                    }
                }
                
            }
            
            
            
            LetterPairs.Clear();
            
            return validKey;
            
        }
        byte[] ReplaceChars(byte[] Message)
        {
            byte[] Encrypted = new byte[Message.Length];
            for (int i = 0; i < Message.Length; i++)
            {
                if (Message[i] > 64 && Message[i] < 91)
                {
                    LetterPairs.TryGetValue((char)Message[i], out char replacement);
                    Encrypted[i] = (byte)replacement;
                }
                else if (Message[i] > 96 && Message[i] < 123)
                {
                    char ToSearch = Convert.ToChar(((char)Message[i]).ToString().ToUpper());
                    LetterPairs.TryGetValue(ToSearch, out char replacement);
                    
                    Encrypted[i] = (byte)Convert.ToChar(replacement.ToString().ToLower());
                }
                else
                {
                    Encrypted[i] = Message[i];
                }
            }
            return Encrypted;
        }
        void FillDictionary(bool cipher)
        {
            int Achar = 65;
            if (cipher)
            {
                for (int i = 0; i < Key.Length; i++)
                {
                    LetterPairs.Add((char)Achar, Key[i]);
                    Achar++;

                }
                for (int i = 65; i < 91; i++)
                {
                    if (!LetterPairs.ContainsValue((char)i))
                    {
                        LetterPairs.Add((char)Achar, (char)i);
                        Achar++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Key.Length; i++)
                {
                    LetterPairs.Add(Key[i], (char)Achar);
                    Achar++;

                }
                for (int i = 65; i < 91; i++)
                {
                    if (!LetterPairs.ContainsKey((char)i))
                    {
                        LetterPairs.Add((char)i, (char)Achar);
                        Achar++;
                    }
                }
            }
           
        }

        
        public bool Decipher(string route, out byte[] Message, string _key)
        {
            Message = null;
            byte[] CipheredMsg;
            bool validKey = SetKey(_key);
            
            if (validKey)
            {
                using (FileStream fs = File.OpenRead(route))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        int counter = 0;
                        while (counter<fs.Length)
                        {
                            CipheredMsg = reader.ReadBytes(1000);
                            FillDictionary(false);
                            Message = ReplaceChars(CipheredMsg);
                            counter += 100;
                        }
                    }
                }
                
            }
            
            

            LetterPairs.Clear();
            
            return validKey;
        }

        
    }
}
