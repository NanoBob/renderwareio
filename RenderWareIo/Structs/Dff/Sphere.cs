using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Sphere : IBinaryStructure<Sphere>
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }
        public int HasPosition { get; set; }
        public int HasNormals { get; set; }


        public uint ContentByteCount => 24;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount;

        public Sphere()
        {
            this.Position = Vector3.Zero;
            this.Radius = 1;
            this.HasPosition = 1;
            this.HasNormals = 0;
        }

        public Sphere Read(Stream stream)
        {
            this.Position = RenderWareFileHelper.ReadVector(stream);
            this.Radius = RenderWareFileHelper.ReadFloat(stream);
            this.HasPosition = (int)RenderWareFileHelper.ReadUint32(stream);
            this.HasNormals = (int)RenderWareFileHelper.ReadUint32(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteVector(stream, this.Position);
            RenderWareFileHelper.WriteFloat(stream, this.Radius);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.HasPosition);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.HasNormals);
        }
    }
}
