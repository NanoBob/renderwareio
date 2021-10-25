using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Txd
{
    public class Txd : IBinaryStructure<Txd>
    {
        public ChunkHeader Header { get; set; }
        public TextureContainer TextureContainer { get; set; }

        public uint ContentByteCount => (uint)(this.TextureContainer.ByteCountWithHeader);
        public uint ByteCount => (uint)(ContentByteCount);
        public uint ByteCountWithHeader => ByteCount + 12 + 24;

        public Txd()
        {
            this.Header = new ChunkHeader(22);
            this.TextureContainer = new TextureContainer();
        }

        public Txd Read(Stream stream)
        {
            try
            {
                this.Header = new ChunkHeader().Read(stream);
                this.TextureContainer = new TextureContainer().Read(stream);
            } catch (Exception e)
            {
                Console.WriteLine("An exception occurred whilst reading");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            this.TextureContainer.Write(stream);
        }
    }
}
