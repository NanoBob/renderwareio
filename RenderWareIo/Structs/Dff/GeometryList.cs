using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class GeometryList : IBinaryStructure<GeometryList>
    {
        public ChunkHeader Header { get; set; }
        public ChunkHeader StructHeader { get; set; }
        public int GeometryCount { get; set; }
        public List<Geometry> Geometries { get; set; }


        public uint ContentByteCount => 4;
        public uint ByteCount => (uint)(ContentByteCount + 12 + Geometries.Sum(geometry => geometry.ByteCountWithHeader));
        public uint ByteCountWithHeader => ByteCount + 12;

        public GeometryList()
        {
            this.Header = new ChunkHeader(26);
            this.StructHeader = new ChunkHeader(1);
            this.GeometryCount = 0;
            this.Geometries = new List<Geometry>();
        }

        public GeometryList Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            this.StructHeader = new ChunkHeader().Read(stream);
            this.GeometryCount = (int)RenderWareFileHelper.ReadUint32(stream);
            this.Geometries = RenderWareFileHelper.ReadBinaryStructure<Geometry>(stream, this.GeometryCount);

            return this;
        }

        public void Write(Stream stream)
        {
            this.StructHeader.Size = ContentByteCount;
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            this.StructHeader.Write(stream);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Geometries.Count);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.Geometries);
        }
    }
}
