using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Txd
{
    public class TextureExtraInfo: IBinaryStructure<TextureExtraInfo>
    {
        public ChunkHeader Header { get; set; }
        public byte[] Data { get; set; }

        public TextureExtraInfo()
        {
            this.Header = new ChunkHeader(3);
            this.Data = new byte[0];
        }

        public TextureExtraInfo Read(Stream stream)
        {
            if (stream.Position == stream.Length)
            {
                this.Header = new ChunkHeader(3);
                this.Data = new byte[0];
                Console.WriteLine("Encountered stream end instead of texture extra info");
                return this;
            }

            this.Header = new ChunkHeader().Read(stream);
            this.Data = new byte[this.Header.Size];
            for (int i = 0; i < this.Data.Length; i++)
            {
                this.Data[i] = RenderWareFileHelper.ReadByte(stream);
            }

            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Write(stream);
            foreach (byte dataByte in this.Data)
            {
                RenderWareFileHelper.WriteByte(stream, dataByte);
            }
        }

        public static TextureExtraInfo Empty()
        {
            return new TextureExtraInfo()
            {
                Header = new ChunkHeader(3),
                Data = new byte[0]
            };
        }
    }
}
