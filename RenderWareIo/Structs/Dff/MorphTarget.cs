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
    public class MorphTarget : IBinaryStructure<MorphTarget>
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }

        public uint HasPosition { get; set; }
        public uint HasNormals { get; set; }


        public uint ContentByteCount => (uint)(Vertices.Count * 12 + Normals.Count * 12 + 8);
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount;

        public MorphTarget()
        {
            this.Vertices = new List<Vector3>();
            this.Normals = new List<Vector3>();
        }
        
        public MorphTarget Read(Stream stream, int vertexCount)
        {
            this.HasPosition = RenderWareFileHelper.ReadUint32(stream);
            this.HasNormals = RenderWareFileHelper.ReadUint32(stream);

            this.Vertices = new List<Vector3>();
            if (this.HasPosition == 1)
                for (int i = 0; i < vertexCount; i++)
                    this.Vertices.Add(RenderWareFileHelper.ReadVector(stream));

            this.Normals = new List<Vector3>();
            if (this.HasNormals == 1)
                for (int i = 0; i < vertexCount; i++)
                    this.Normals.Add(RenderWareFileHelper.ReadVector(stream));

            return this;
        }

        public MorphTarget Read(Stream stream)
        {
            throw new NotImplementedException("Morph target Read method requires more parameters.");
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteUint32(stream, (uint)(this.Vertices.Any() ? 1 : 0));
            RenderWareFileHelper.WriteUint32(stream, (uint)(this.Normals.Any() ? 1 : 0));

            foreach (Vector3 vector in this.Vertices)
                RenderWareFileHelper.WriteVector(stream, vector);

            foreach (Vector3 vector in this.Normals)
            {
                RenderWareFileHelper.WriteVector(stream, vector);
            }
        }
    }
}
