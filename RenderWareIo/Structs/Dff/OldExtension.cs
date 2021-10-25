using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Linq;

namespace RenderWareIo.Structs.Dff
{
    public class OldExtension : IBinaryStructure<OldExtension>
    {
        public ChunkHeader Header { get; set; }
        public byte[] Data { get; set; }
        public int Size => Data.Length;
        public List<IByteCountStructure> Structures { get; set; }


        public uint ContentByteCount => (uint)Size;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount + 12;

        public OldExtension()
        {
            this.Header = new ChunkHeader(3);
            this.Data = new byte[0];
            this.Structures = new List<IByteCountStructure>();
        }

        public OldExtension Read(Stream stream)
        {
            if (stream.Position == stream.Length || stream.Position > stream.Length - 12)
            {
                this.Header = new ChunkHeader(3);
                this.Data = new byte[0];
                Console.WriteLine("Encountered stream end instead of dff extension");
                return this;
            }

            this.Header = new ChunkHeader().Read(stream);

            if (Header.Type != 3)
            {
                if (Header.Type == 1)
                {
                    stream.Position -= 12;
                }
                //throw new Exception($"Encountered header type {Header.Type} instead of 3 at position {stream.Position - 12}");
                return this;
            }

            this.Data = new byte[this.Header.Size];
            for (int i = 0; i < this.Data.Length; i++)
            {
                this.Data[i] = RenderWareFileHelper.ReadByte(stream);
            }

            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            foreach (byte dataByte in this.Data)
            {
                RenderWareFileHelper.WriteByte(stream, dataByte);
            }
        }

        public static OldExtension Empty()
        {
            return new OldExtension()
            {
                Header = new ChunkHeader(3),
                Data = new byte[0]
            };
        }
    }
}
