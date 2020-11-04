using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CypherClasses
{
    public class ZigZag : ICipher
    {
        int Levels = 0;
        List<byte>[] Characters;
        bool validLevel = false;
       byte ToFillChar;
        public bool SetLevels(int levels)
        {
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
        public bool Cipher(string route, out byte[] CipheredMsg)
        {
            byte[] message;
            using (FileStream fs = File.OpenRead(route))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    message = reader.ReadBytes(Convert.ToInt32(fs.Length));
                }
            }
            if (validLevel)
            {
                Characters = new List<byte>[Levels];
                
                
                CipheredMsg = LevelReading(ZigZagDistribution(message));
            }
            else CipheredMsg = null;
            
            return validLevel;
        }
        int ZigZagDistribution(byte[] Message)
        {
            int ToSubstract = 0;
            int ToAssignLevel = 0;
            bool add = false;
            int WaveSize = 2*(Levels - 1);
            int FillCharsQ = WaveSize - (Message.Length % WaveSize);
            FindFillChar(Message);
            //Se crea nuevo vector con el mensaje original pero agregando los caracteres de relleno
            byte[] NewText = new byte[Message.Length + FillCharsQ];
            for (int i = 0; i < Message.Length; i++)
            {
                NewText[i] = Message[i];
            }
            for (int i = Message.Length; i < NewText.Length; i++)
            {
                NewText[i] = ToFillChar;
            }
            //Se inicializan las Listas
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i] = new List<byte>();
            }
            //Se distribuyen caracteres en zigzag
            for (int i = 0; i < NewText.Length; i++)
            {
                ToAssignLevel = i - ToSubstract;
                if (ToAssignLevel == (Levels - 1)) add = true;
                if (ToAssignLevel == 0) add = false;
                if (add) ToSubstract += 2;
                Characters[ToAssignLevel].Add(NewText[i]);
            }
            return NewText.Length;
        }
        void FindFillChar(byte[] message)
        {
           
            int fillChar = -1;
            for (int i = 0; i < 256; i++)
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
            if (fillChar != -1)
            {
                ToFillChar = (byte)fillChar;
            }
            //else ¿Qué pasa si el texto incluye todos los caracteres?

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
        public bool Decipher(string route,  out byte[] Message)
        {
            byte[] CipheredMsg;
            using (FileStream fs = File.OpenRead(route))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    CipheredMsg = reader.ReadBytes(Convert.ToInt32(fs.Length));
                }
            }
            int CompleteWaves = (CipheredMsg.Length) / (2 * (Levels - 1));
            int Residue = (CipheredMsg.Length) % (2 * (Levels - 1));
            Characters = new List<byte>[Levels];
            if (validLevel && Residue == 0)
            {
                HorizontalDistribution(CipheredMsg, CompleteWaves);
                Message = ZigZagReading(CipheredMsg.Length);
                return true;
            }
            else
            {
                Message = null;
                return false;
            }
        }
        void HorizontalDistribution(byte[] _message, int completeWaves)
        {
            
            //Se inicializan las Listas
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i] = new List<byte>();
            }
            int index = 0;
            for (int i = 0; i < Characters.Length; i++)
            {
                if (i>0 && i<(Characters.Length-1))
                {
                    for (int j = 0; j < 2*completeWaves; j++)
                    {

                        Characters[i].Add(_message[index]);
                        index++;
                    }
                }
                else
                {
                    for (int j = 0; j < completeWaves; j++)
                    {
                        
                        Characters[i].Add(_message[index]);
                        index++;
                    }
                }
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
