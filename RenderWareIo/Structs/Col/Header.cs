using RenderWareIo.Constants;
using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Header : IBinaryStructure<Header>
    {
        //public static int Size => 116;

        // version 1
        public char[] FourCC { get; set; }
        public uint Size { get; set; }
        public char[] Name { get; set; }
        public short ModelId { get; set; }
        public Bounds Bounds { get; set; }

        // version 2
        public ushort SphereCount { get; set; }
        public ushort BoxCount { get; set; }
        public ushort FaceCount { get; set; }
        public byte LineCount { get; set; }

        public byte Padding { get; set; }
        public uint Flags { get; set; }

        public uint SphereOffset { get; set; }
        public uint BoxOffset { get; set; }
        public uint LineOffset { get; set; }
        public uint VertexOffset { get; set; }
        public uint FaceOffset { get; set; }
        public uint TrianglePlaneOffset { get; set; }

        // version 3
        public uint ShadowMeshFaceCount { get; set; }
        public uint ShadowMeshVertexOffset { get; set; }
        public uint ShadowMeshFaceOffset { get; set; }

        public int ColVersion =>
            this.FourCC.Select(x => (byte)x).SequenceEqual(ColConstants.Col2) ? 2 :
            this.FourCC.Select(x => (byte)x).SequenceEqual(ColConstants.Col3) ? 3 :
            1;

        public Header Read(Stream stream)
        {
            this.FourCC = RenderWareFileHelper.ReadChars(stream, 4);
            this.Size = RenderWareFileHelper.ReadUint32(stream);
            this.Name = RenderWareFileHelper.ReadChars(stream, 22);
            this.ModelId = (short)RenderWareFileHelper.ReadUint16(stream);
            this.Bounds = (new Bounds()).Read(stream);

            this.SphereCount = RenderWareFileHelper.ReadUint16(stream);
            this.BoxCount = RenderWareFileHelper.ReadUint16(stream);
            this.FaceCount = RenderWareFileHelper.ReadUint16(stream);
            this.LineCount = RenderWareFileHelper.ReadByte(stream);

            this.Padding = RenderWareFileHelper.ReadByte(stream);
            this.Flags = RenderWareFileHelper.ReadUint32(stream);

            this.SphereOffset = RenderWareFileHelper.ReadUint32(stream);
            this.BoxOffset = RenderWareFileHelper.ReadUint32(stream);
            this.LineOffset = RenderWareFileHelper.ReadUint32(stream);
            this.VertexOffset = RenderWareFileHelper.ReadUint32(stream);
            this.FaceOffset = RenderWareFileHelper.ReadUint32(stream);
            this.TrianglePlaneOffset = RenderWareFileHelper.ReadUint32(stream);

            if (this.ColVersion >= 3)
            {
                this.ShadowMeshFaceCount = RenderWareFileHelper.ReadUint32(stream);
                this.ShadowMeshVertexOffset = RenderWareFileHelper.ReadUint32(stream);
                this.ShadowMeshFaceOffset = RenderWareFileHelper.ReadUint32(stream);
            }

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteChars(stream, this.FourCC);
            RenderWareFileHelper.WriteUint32(stream, this.Size);
            RenderWareFileHelper.WriteChars(stream, this.Name);
            RenderWareFileHelper.WriteUint16(stream, (ushort)this.ModelId);
            this.Bounds.Write(stream);

            RenderWareFileHelper.WriteUint16(stream, this.SphereCount);
            RenderWareFileHelper.WriteUint16(stream, this.BoxCount);
            RenderWareFileHelper.WriteUint16(stream, this.FaceCount);
            RenderWareFileHelper.WriteByte(stream, this.LineCount);

            RenderWareFileHelper.WriteByte(stream, this.Padding);
            RenderWareFileHelper.WriteUint32(stream, this.Flags);

            RenderWareFileHelper.WriteUint32(stream, this.SphereOffset);
            RenderWareFileHelper.WriteUint32(stream, this.BoxOffset);
            RenderWareFileHelper.WriteUint32(stream, this.LineOffset);
            RenderWareFileHelper.WriteUint32(stream, this.VertexOffset);
            RenderWareFileHelper.WriteUint32(stream, this.FaceOffset);
            RenderWareFileHelper.WriteUint32(stream, this.TrianglePlaneOffset);

            if (this.ColVersion >= 3)
            {
                RenderWareFileHelper.WriteUint32(stream, this.ShadowMeshFaceCount);
                RenderWareFileHelper.WriteUint32(stream, this.ShadowMeshVertexOffset);
                RenderWareFileHelper.WriteUint32(stream, this.ShadowMeshFaceOffset);
            }
        }

        public Bounds GenerateBounds(Body body)
        {
            var min = new Vector3(body.Vertices.Min(v => v.FirstFloat), body.Vertices.Min(v => v.SecondFloat), body.Vertices.Min(v => v.ThirdFloat));
            var max = new Vector3(body.Vertices.Max(v => v.FirstFloat), body.Vertices.Max(v => v.SecondFloat), body.Vertices.Max(v => v.ThirdFloat));
            var center = min + (max - min) * 0.5f;
            var radius = body.Vertices.Max(vertex => (new Vector3(vertex.FirstFloat, vertex.SecondFloat, vertex.ThirdFloat) - center).Length());

            return new Bounds()
            {
                Min = min,
                Max = max,
                Center = center,
                Radius = radius
            };
        }

        public void Generate(Body body)
        {
            this.Bounds = GenerateBounds(body);

            this.FourCC = new char[] { 'C', 'O', 'L', '3' };
            this.Name = new char[] { 'c', 'o', 'l', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' };

            this.SphereCount = (ushort)body.Spheres.Count;
            this.BoxCount = (ushort)body.Boxes.Count;
            this.FaceCount = (ushort)body.Faces.Count;
            this.LineCount = 0; // TODO: Lines

            this.Padding = 0;

            this.SphereOffset = (uint)116;
            this.BoxOffset = (uint)(this.SphereOffset + this.SphereCount * (new Sphere()).Size);
            this.LineOffset = (uint)(this.BoxOffset + this.BoxCount * (new Box()).Size);
            this.VertexOffset = (uint)(this.LineOffset + this.LineCount * 0);   // TODO: Lines
            this.FaceOffset = (uint)(this.VertexOffset + body.Vertices.Count * (new Vertex()).Size);
            this.TrianglePlaneOffset = (uint)(this.FaceOffset + this.FaceCount * (new Face()).Size);

            this.ShadowMeshFaceCount = (uint)body.ShadowMeshFaces.Count;
            this.ShadowMeshVertexOffset = (uint)(this.TrianglePlaneOffset + 0); // TODO: triangle planes
            this.ShadowMeshFaceOffset = (uint)(this.ShadowMeshFaceOffset + body.ShadowMeshVertices.Count * (new Vertex()).Size);
            this.Size = body.Size + 64 + 48;

        }
    }
}
