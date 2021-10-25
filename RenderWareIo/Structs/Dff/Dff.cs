using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Dff : IBinaryStructure<Dff>
    {
        public ChunkHeader Header { get; set; }
        public Clump Clump { get; set; }


        public uint ContentByteCount => Clump.ByteCountWithHeader;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount;

        public Dff ()
        {
            this.Header = new ChunkHeader(0x10);
            this.Clump = new Clump();
        }

        public Dff Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            while (this.Header.Type != 0x10)
            {
                stream.Position += this.Header.Size;
                this.Header = new ChunkHeader().Read(stream);
            }
            this.Clump = new Clump().Read(stream);
            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Size = ByteCount;
            this.Header.Write(stream);
            this.Clump.Write(stream);
        }
    }
}
