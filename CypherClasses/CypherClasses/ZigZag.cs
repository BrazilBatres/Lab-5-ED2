using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CypherClasses
{
    public class ZigZag : ICipher<int>
    {
        int Levels = 0;
        List<byte>[] Characters;
        //bool validLevel = false;
       byte ToFillChar;
        public bool SetLevels(int levels)
        {
            bool validLevel;
            if (levels > 0)
            {
                Levels = levels;
                validLevel = true;
            }
            else validLevel = false;
            return validLevel;
        }
        public void SetFillChar(byte toFill)
        {
            ToFillChar = toFill;
        }
        public bool Cipher(string route, out byte[] CipheredMsg, int levels)
        {
            //byte[] message;
            bool validLevel = SetLevels(levels);
            //using (FileStream fs = File.OpenRead(route))
            //{
            //    using (BinaryReader reader = new BinaryReader(fs))
            //    {
            //        message = reader.ReadBytes(Convert.ToInt32(fs.Length));
            //    }
            //}
            if (validLevel)
            {
                Characters = new List<byte>[Levels];
                CipheredMsg = LevelReading(ZigZagDistribution(route));
            }
            else CipheredMsg = null;
            
            return validLevel;
        }
        int ZigZagDistribution(string route)
        {
            int ToSubstract = 0;
            int ToAssignLevel = 0;
            bool add = false;
            int WaveSize = 2*(Levels - 1);
            int toReturn = 0;
            int FillCharsQ;
            //Se inicializan las Listas
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i] = new List<byte>();
            }
            using (FileStream fs = File.OpenRead(route))
            {
                toReturn = Convert.ToInt32(fs.Length);
                FillCharsQ = WaveSize - (Convert.ToInt32(fs.Length) % WaveSize);
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    int counter = 0;
                    while (counter < fs.Length)
                    {
                        byte[] Message = reader.ReadBytes(5);
                        FindFillChar(Message);
                        //Se distribuyen caracteres en zigzag
                        for (int i = 0; i < Message.Length; i++)
                        {
                            ToAssignLevel = counter + i - ToSubstract;
                            if (ToAssignLevel == (Levels - 1)) add = true;
                            if (ToAssignLevel == 0) add = false;
                            if (add) ToSubstract += 2;
                            Characters[ToAssignLevel].Add(Message[i]);
                        }
                        counter += 5;
                    }
                    
                }
            }
            
            
            //Se crea nuevo vector con el mensaje original pero agregando los caracteres de relleno
            //byte[] NewText = new byte[Message.Length + FillCharsQ];
            //for (int i = 0; i < Message.Length; i++)
            //{
            //    NewText[i] = Message[i];
            //}

            //for (int i = Message.Length; i < NewText.Length; i++)
            //{
            //    NewText[i] = ToFillChar;
            //}
            if (FillCharsQ > 0)
            {
                
                for (int i = 0; i <FillCharsQ; i++)
                {
                    ToAssignLevel = toReturn + i - ToSubstract;
                    if (ToAssignLevel == (Levels - 1)) add = true;
                    if (ToAssignLevel == 0) add = false;
                    if (add) ToSubstract += 2;
                    Characters[ToAssignLevel].Add(ToFillChar);
                }
            }
            
            
            return toReturn + FillCharsQ;
        }
        void FindFillChar(byte[] message)
        {
           
            int fillChar = ToFillChar;
            for (int i = fillChar; i < 256; i++)
            {
                 bool NotInMsg = true;
                for (int j = 0; j < message.Length; j++)
                {
                    if (message[j] == i)
                    {
                        NotInMsg = false;
                        break;
                    }
                }
                if (NotInMsg)
                {
                    fillChar = i;
                    break;
                }
            }
            ToFillChar = (byte)fillChar;
            

        }
        byte[] LevelReading(int length)
        {
            byte[] cipheredMsg = new byte[length];
            int counter = 0;
            for (int i = 0; i < Levels; i++)
            {
                foreach (var item in Characters[i])
                {
                    cipheredMsg[counter] = item;
                    counter++;
                }
            }
            return cipheredMsg;
        }
        public byte GetFillChar()
        {
            return ToFillChar;
        }
        public bool Decipher(string route,  out byte[] Message, int levels)
        {
            //byte[] CipheredMsg;
            bool validLevel = SetLevels(levels);
            //using (FileStream fs = File.OpenRead(route))
            //{
            //    using (BinaryReader reader = new BinaryReader(fs))
            //    {
            //        CipheredMsg = reader.ReadBytes(Convert.ToInt32(fs.Length));
            //    }
            //}
            
            Characters = new List<byte>[Levels];
            int messageLength = HorizontalDistribution(route, out int Residue);
            if (validLevel && Residue == 0)
            {
                
                Message = ZigZagReading(messageLength);
                return true;
            }
            else
            {
                Message = null;
                return false;
            }
        }
        int HorizontalDistribution(string route, out int Residue)
        {
            int ToReturn = 0;
            //Se inicializan las Listas
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i] = new List<byte>();
            }
            using (FileStream fs = File.OpenRead(route))
            {
                ToReturn = Convert.ToInt32(fs.Length);
                int completeWaves = (ToReturn) / (2 * (Levels - 1));
                Residue = (ToReturn) % (2 * (Levels - 1));
                if (Residue == 0)
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        int counter = 0;
                        int index = 0;
                        int level = 0;
                        bool levelchange = false;
                        while (counter < fs.Length)
                        {
                            byte[] _message = reader.ReadBytes(1000);

                            for (int i = 0; i < _message.Length; i++)
                            {
                                if (level <= Levels)
                                {
                                    if (level > 0 && level < (Characters.Length - 1))
                                    {
                                        bool firstIteration = true;
                                        for (int j = index; j < 2 * completeWaves; j++)
                                        {

                                            if (!firstIteration) i++;
                                            else firstIteration = false;
                                            if (i < _message.Length)
                                            {
                                                Characters[level].Add(_message[i]);

                                                index++;
                                            }
                                            else break;
                                        }
                                        if (index == 2*completeWaves)
                                        {
                                            level++;
                                            index = 0;
                                            //levelchange = true;
                                        }
                                    }
                                    else
                                    {
                                        bool firstIteration = true;
                                        for (int j = index; j < completeWaves; j++)
                                        {
                                            if (!firstIteration) i++;
                                            else firstIteration = false;
                                            if (i < _message.Length)
                                            {
                                                Characters[level].Add(_message[i]);

                                                index++;
                                            }
                                            else break;
                                        }
                                        if (index == completeWaves)
                                        {
                                            level++;
                                            index = 0;
                                            //levelchange = true;
                                        }

                                    }
                                }
                            }

                            counter += 1000;
                        }

                    }
                }
                return ToReturn;
            }
            


        }
        byte[] ZigZagReading(int Msglength)
        {
            int ToSubstract = 0;
            int ToGetLevel = 0;
            bool add = false;
            List<byte> ResultMsg = new List<byte>();
            for (int i = 0; i < Msglength; i++)
            {
                ToGetLevel = i - ToSubstract;
                if (ToGetLevel == (Levels - 1)) add = true;
                if (ToGetLevel == 0) add = false;
                if (add) ToSubstract += 2;
                byte ToMsg = Characters[ToGetLevel][0];
                if (ToMsg != ToFillChar) ResultMsg.Add(ToMsg);
                Characters[ToGetLevel].RemoveAt(0);
            }

            return ResultMsg.ToArray();
        }
    }
}
