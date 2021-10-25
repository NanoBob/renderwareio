using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Box : IBinaryStructure<Box>
    {
        public uint Size => 28;

        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }
        public Surface Surface { get; set; }

        public Box Read(Stream stream)
        {
            this.Min = RenderWareFileHelper.ReadVector(stream);
            this.Max = RenderWareFileHelper.ReadVector(stream);
            this.Surface = new Surface().Read(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteVector(stream, this.Min);
            RenderWareFileHelper.WriteVector(stream, this.Max);
            this.Surface.Write(stream);
        }
    }
}
