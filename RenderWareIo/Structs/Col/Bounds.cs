using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Bounds : IBinaryStructure<Bounds>
    {
        public static int Size => 40;

        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }
        public Vector3 Center { get; set; }
        public float Radius { get; set; }

        public Bounds Read(Stream stream)
        {
            this.Min = RenderWareFileHelper.ReadVector(stream);
            this.Max = RenderWareFileHelper.ReadVector(stream);
            this.Center = RenderWareFileHelper.ReadVector(stream);
            this.Radius = RenderWareFileHelper.ReadFloat(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteVector(stream, this.Min);
            RenderWareFileHelper.WriteVector(stream, this.Max);
            RenderWareFileHelper.WriteVector(stream, this.Center);
            RenderWareFileHelper.WriteFloat(stream, this.Radius);
        }
    }
}
