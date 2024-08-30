using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Geometry : IBinaryStructure<Geometry>
    {
        public ChunkHeader Header { get; set; }
        public ChunkHeader StructHeader { get; set; }
        public uint Flags { get; set; }
        public int TriangleCount { get; set; }
        public int VertexCount { get; set; }
        public int MorphTargetCount { get; set; }
        public List<Color> Colors { get; set; }
        public List<Uv> TexCoords { get; set; }
        public List<Triangle> Triangles { get; set; }
        public Sphere Sphere { get; set; }
        public List<MorphTarget> MorphTargets { get; set; }
        public MaterialList MaterialList { get; set; }
        public Extension Extension { get; set; }


        public uint ContentByteCount => (uint)(
            4 + 4 + 4 + 4 + 
            Colors.Sum(color => color.ByteCountWithHeader) + 
            TexCoords.Sum(uv => uv.ByteCountWithHeader) + 
            Triangles.Sum(triangle => triangle.ByteCountWithHeader) + 
            Sphere.ByteCountWithHeader + 
            MorphTargets.Sum(morphTarget => morphTarget.ByteCountWithHeader)
        );

        public uint ByteCount => (uint)(ContentByteCount + Extension.ByteCount + 2 * 12) + MaterialList.ByteCountWithHeader;
        public uint ByteCountWithHeader => ByteCount + 12;

        public Geometry()
        {
            this.Header = new ChunkHeader(15);
            this.StructHeader = new ChunkHeader(1);
            this.Flags = 0;
            this.TriangleCount = 0;
            this.VertexCount = 0;
            this.MorphTargetCount = 0;
            this.Colors = new List<Color>();
            this.TexCoords = new List<Uv>();
            this.Triangles = new List<Triangle>();
            this.Sphere = new Sphere();
            this.MorphTargets = new List<MorphTarget>();
            this.MaterialList = new MaterialList();
            this.Extension = new Extension();
        }

        public Geometry Read(Stream stream)
        {
            long start = stream.Position;
            this.Header = new ChunkHeader().Read(stream);
            this.StructHeader = new ChunkHeader().Read(stream);
            this.Flags = RenderWareFileHelper.ReadUint32(stream);
            this.TriangleCount = (int)RenderWareFileHelper.ReadUint32(stream);
            this.VertexCount = (int)RenderWareFileHelper.ReadUint32(stream);
            this.MorphTargetCount = (int)RenderWareFileHelper.ReadUint32(stream);

            if ((this.Flags & 0x08) != 0)
            {
                this.Colors = RenderWareFileHelper.ReadBinaryStructure<Color>(stream, this.VertexCount);
            } else
            {
                this.Colors = new List<Color>();
            }

            if ((this.Flags & 0x04) != 0 || (this.Flags & 0x80) != 0)
            {
                this.TexCoords = RenderWareFileHelper.ReadBinaryStructure<Uv>(stream, this.VertexCount);
            } else
            {
                this.TexCoords = new List<Uv>();
            }

            this.Triangles = RenderWareFileHelper.ReadBinaryStructure<Triangle>(stream, this.TriangleCount);
            this.Sphere = new Sphere().Read(stream);
            this.ReadMorphTargets(stream);

            // Temporary fix until I figure out what the missing data here is
            stream.Position = start + this.StructHeader.Size + 24;
            this.MaterialList = new MaterialList().Read(stream);
            this.Extension = new Extension().Read(stream);

            return this;
        }

        private void ReadMorphTargets(Stream stream)
        {
            this.MorphTargets = new List<MorphTarget>();
            for (int i = 0; i < this.MorphTargetCount; i++)
            {
                MorphTargets.Add(new MorphTarget().Read(stream, this.VertexCount));
            }
        }

        public void Write(Stream stream)
        {
            this.StructHeader.Size = ContentByteCount;
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            this.StructHeader.Write(stream);

            RenderWareFileHelper.WriteUint32(stream, this.Flags);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Triangles.Count);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.MorphTargets.Sum(m => m.Vertices.Count));
            RenderWareFileHelper.WriteUint32(stream, (uint)this.MorphTargets.Count);

            RenderWareFileHelper.WriteBinaryStructure(stream, this.Colors);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.TexCoords);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.Triangles);

            this.Sphere.Write(stream);
            this.WriteMorphTargets(stream);

            this.MaterialList.Write(stream);
            this.Extension.Write(stream);
        }

        private void WriteMorphTargets(Stream stream)
        {
            foreach(MorphTarget target in this.MorphTargets)
            {
                target.Write(stream);
            }
        }
    }
}
