using RenderWareIo.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.ReadWriteHelpers
{
    public static class RenderWareFileHelper
    {
        public static byte ReadByte(Stream stream)
        {
            byte[] buffer = new byte[1];
            int bytesRead = stream.Read(buffer, 0, 1);
            if (bytesRead != 1)
            {
                throw new IOException("Unable to read byte from memory stream");
            }
            return buffer[0];
        }

        public static void WriteByte(Stream stream, byte value)
        {
            stream.Write(new byte[] { value }, 0, 1);
        }

        public static ushort ReadUint16(Stream stream)
        {
            byte[] buffer = new byte[2];
            int bytesRead = stream.Read(buffer, 0, 2);
            if (bytesRead != 2)
            {
                throw new IOException("Unable to read uint16 from memory stream");
            }
            return BitConverter.ToUInt16(buffer, 0);
        }

        public static void WriteUint16(Stream stream, ushort value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 2);
        }

        public static uint ReadUint32(Stream stream)
        {
            byte[] buffer = new byte[4];
            int bytesRead = stream.Read(buffer, 0, 4);
            if (bytesRead != 4)
            {
                throw new IOException("Unable to read uint32 from memory stream");
            }
            return BitConverter.ToUInt32(buffer, 0);
        }

        public static void WriteUint32(Stream stream, uint value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 4);
        }

        public static float ReadFloat(Stream stream)
        {
            byte[] buffer = new byte[4];
            int bytesRead = stream.Read(buffer, 0, 4);
            if (bytesRead != 4)
            {
                throw new IOException("Unable to read float from memory stream");
            }
            return BitConverter.ToSingle(buffer, 0);
        }

        public static void WriteFloat(Stream stream, float value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 4);
        }

        public static Vector3 ReadVector(Stream stream)
        {
            return new Vector3(
                ReadFloat(stream),
                ReadFloat(stream),
                ReadFloat(stream)
            );
        }

        public static void WriteVector(Stream stream, Vector3 value)
        {
            WriteFloat(stream, value.X);
            WriteFloat(stream, value.Y);
            WriteFloat(stream, value.Z);
        }

        public static char[] ReadChars(Stream stream, int count)
        {
            char[] chars = new char[count];
            for (int i = 0; i < count; i++)
            {
                chars[i] = (char)ReadByte(stream);
            }
            return chars;
        }

        public static void WriteChars(Stream stream, char[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                WriteByte(stream, (byte)value[i]);
            }
        }

        public static string ReadString(Stream stream, char terminator = '\0', int padTo = 4)
        {
            string value = "";
            char lastChar = ' ';
            while(lastChar != terminator)
            {
                lastChar = (char)ReadByte(stream);
                value += lastChar;
            }

            if (padTo > 0 && value.Length % padTo > 0)
            {
                int remainder = padTo - (value.Length % padTo);
                for (int i = 0; i < remainder; i++)
                {
                    ReadByte(stream);
                }
            }

            return value;
        }

        public static void WriteString(Stream stream, string value, int padTo = 4)
        {
            WriteChars(stream, value.ToCharArray());

            if (padTo > 0 && value.Length % padTo > 0)
            {
                int remainder = padTo - (value.Length % padTo);
                for (int i = 0; i < remainder; i++)
                {
                    WriteByte(stream, 0);
                }
            }
        }

        public static List<T> ReadBinaryStructure<T>(Stream stream, int count) where T : IBinaryStructure<T>, new()
        {
            List<T> list = new List<T>();

            for (int i = 0; i < count; i++)
            {
                list.Add(new T().Read(stream));
            }

            return list;
        }

        public static void WriteBinaryStructure<T>(Stream stream, List<T> binaryStructures) where T : IBinaryStructure<T>
        {
            foreach (T structure in binaryStructures)
            {
                structure.Write(stream);
            }
        }
    }
}
