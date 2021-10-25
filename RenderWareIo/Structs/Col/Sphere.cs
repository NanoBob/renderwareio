using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Sphere : IBinaryStructure<Sphere>
    {
        public uint Size => 20;

        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public Surface Surface { get; set; }

        public Sphere Read(Stream stream)
        {
            this.Center = RenderWareFileHelper.ReadVector(stream);
            this.Radius = RenderWareFileHelper.ReadFloat(stream);
            this.Surface = new Surface().Read(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteVector(stream, this.Center);
            RenderWareFileHelper.WriteFloat(stream, this.Radius);
            this.Surface.Write(stream);
        }
    }
}
