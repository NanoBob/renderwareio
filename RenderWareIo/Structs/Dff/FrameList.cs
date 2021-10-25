using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class FrameList : IBinaryStructure<FrameList>
    {
        public ChunkHeader Header { get; set; }
        public ChunkHeader StructHeader { get; set; }
        public int FrameCount { get; set; }
        public List<Frame> Frames { get; set; }
        public List<Extension> Extensions { get; set; }


        public uint ContentByteCount => (uint)(4 + Frames.Sum(frame => frame.ByteCountWithHeader));
        public uint ByteCount => (uint)(ContentByteCount + Extensions.Sum(extension => extension.ByteCountWithHeader) + 12);
        public uint ByteCountWithHeader => ByteCount + 12;

        public FrameList()
        {
            this.Header = new ChunkHeader(14);
            this.StructHeader = new ChunkHeader(1);
            this.FrameCount = 0;
            this.Frames = new List<Frame>();
            this.Extensions = new List<Extension>();
        }

        public FrameList Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            this.StructHeader = new ChunkHeader().Read(stream);
            this.FrameCount = (int)RenderWareFileHelper.ReadUint32(stream);
            this.Frames = RenderWareFileHelper.ReadBinaryStructure<Frame>(stream, this.FrameCount);

            var header = new ChunkHeader().Read(stream);
            stream.Position -= 12;

            if (header.Type != 3)
            {
                Console.WriteLine($"Encountered header {header.Type} instead of frame list extension (3)");
                this.Extensions = new List<Extension>();
                return this;
            }
            this.Extensions = RenderWareFileHelper.ReadBinaryStructure<Extension>(stream, this.FrameCount);

            return this;
        }

        public void Write(Stream stream)
        {
            this.FrameCount = this.Frames.Count;

            this.StructHeader.Size = ContentByteCount;
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            this.StructHeader.Write(stream);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Frames.Count);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.Frames);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.Extensions);
        }
    }
}
