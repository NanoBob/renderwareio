using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Atomic : IBinaryStructure<Atomic>
    {
        public ChunkHeader Header { get; set; }
        public ChunkHeader StructHeader { get; set; }
        public uint FrameIndex { get; set; }
        public uint GeometryIndex { get; set; }
        public uint Flags { get; set; }
        public uint Unused { get; set; }
        public Extension Extension { get; set; }


        public uint ContentByteCount => 16;
        public uint ByteCount => ContentByteCount + Extension.ByteCountWithHeader + 12;
        public uint ByteCountWithHeader => ByteCount + 12;

        public Atomic()
        {
            this.Header = new ChunkHeader(20);
            this.StructHeader = new ChunkHeader(1);
            this.FrameIndex = 0;
            this.GeometryIndex = 0;
            this.Flags = 4;
            this.Unused = 0;
            this.Extension = new Extension();
        }

        public Atomic Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            this.StructHeader = new ChunkHeader().Read(stream);

            this.FrameIndex = RenderWareFileHelper.ReadUint32(stream);
            this.GeometryIndex = RenderWareFileHelper.ReadUint32(stream);
            this.Flags = RenderWareFileHelper.ReadUint32(stream);
            this.Unused = RenderWareFileHelper.ReadUint32(stream);

            this.Extension = new Extension().Read(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            this.StructHeader.Size = ContentByteCount;
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            this.StructHeader.Write(stream);

            RenderWareFileHelper.WriteUint32(stream, this.FrameIndex);
            RenderWareFileHelper.WriteUint32(stream, this.GeometryIndex);
            RenderWareFileHelper.WriteUint32(stream, this.Flags);
            RenderWareFileHelper.WriteUint32(stream, this.Unused);

            this.Extension.Write(stream);
        }
    }
}
