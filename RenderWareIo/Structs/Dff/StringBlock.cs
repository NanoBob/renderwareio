using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class StringBlock : IBinaryStructure<StringBlock>
    {
        public ChunkHeader Header { get; set; }
        public string Value { get; set; }

        public uint ContentByteCount => (uint)(this.Value.Length % 4 == 0 ? Value.Length : (this.Value.Length / 4 + 1) * 4);
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount + 12;

        public StringBlock()
        {
            this.Header = new ChunkHeader(2);
            this.Value = "";
        }

        public StringBlock Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            this.Value = RenderWareFileHelper.ReadString(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Size = ByteCount;
            this.Header.Write(stream);
            RenderWareFileHelper.WriteString(stream, this.Value);
        }
    }
}
