using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class MorphTarget : IBinaryStructure<MorphTarget>
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }


        public uint ContentByteCount => (uint)(Vertices.Count * 12 + Normals.Count * 12);
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount;

        public MorphTarget()
        {
            this.Vertices = new List<Vector3>();
            this.Normals = new List<Vector3>();
        }
        
        public MorphTarget Read(Stream stream, bool withNormals, int vertexCount)
        {
            this.Vertices = new List<Vector3>();
            for (int i = 0; i < vertexCount; i++)
            {
                this.Vertices.Add(RenderWareFileHelper.ReadVector(stream));
            }

            this.Normals = new List<Vector3>();
            if (withNormals)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    this.Normals.Add(RenderWareFileHelper.ReadVector(stream));
                }
            }

            return this;
        }

        public MorphTarget Read(Stream stream)
        {
            throw new NotImplementedException("Morph target Read method requires more parameters.");
        }

        public void Write(Stream stream)
        {
            foreach (Vector3 vector in this.Vertices)
            {
                RenderWareFileHelper.WriteVector(stream, vector);
            }

            foreach (Vector3 vector in this.Normals)
            {
                RenderWareFileHelper.WriteVector(stream, vector);
            }
        }
    }
}
