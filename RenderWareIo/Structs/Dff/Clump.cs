using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Clump : IBinaryStructure<Clump>
    {
        public ChunkHeader Header { get; set; }
        public int AtomicCount { get; set; }
        public int LightCount { get; set; }
        public int CameraCount { get; set; }
        public FrameList FrameList { get; set; }
        public GeometryList GeometryList { get; set; }
        public List<Atomic> Atomics { get; set; }
        public Extension Extension { get; set; }


        public uint ContentByteCount => 12;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => (uint)(
            ByteCount + 
            FrameList.ByteCountWithHeader +
            GeometryList.ByteCountWithHeader +
            Atomics.Sum(atomic => atomic.ByteCountWithHeader) +
            Extension.ByteCountWithHeader +
            12);

        public Clump()
        {
            this.Header = new ChunkHeader(1);
            this.AtomicCount = 0;
            this.LightCount = 0;
            this.CameraCount = 0;
            this.FrameList = new FrameList();
            this.GeometryList = new GeometryList();
            this.Atomics = new List<Atomic>();
            this.Extension = new Extension();
        }

        public Clump Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            this.AtomicCount = (int)RenderWareFileHelper.ReadUint32(stream);
            this.LightCount = (int)RenderWareFileHelper.ReadUint32(stream);
            this.CameraCount = (int)RenderWareFileHelper.ReadUint32(stream);
            this.FrameList = new FrameList().Read(stream);
            this.GeometryList = new GeometryList().Read(stream);
            this.Atomics = RenderWareFileHelper.ReadBinaryStructure<Atomic>(stream, this.AtomicCount);
            this.Extension = new Extension().Read(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Atomics.Count);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.LightCount);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.CameraCount);
            this.FrameList.Write(stream);
            this.GeometryList.Write(stream);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.Atomics);
            this.Extension.Write(stream);
        }
    }
}
