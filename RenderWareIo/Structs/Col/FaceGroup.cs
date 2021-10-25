using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class FaceGroup : IBinaryStructure<FaceGroup>
    {
        public uint Size => 28;

        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }
        public ushort StartFace { get; set; }
        public ushort EndFace { get; set; }

        public FaceGroup Read(Stream stream)
        {

            this.Min = RenderWareFileHelper.ReadVector(stream);
            this.Max = RenderWareFileHelper.ReadVector(stream);
            this.StartFace = RenderWareFileHelper.ReadUint16(stream);
            this.EndFace = RenderWareFileHelper.ReadUint16(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteVector(stream, this.Min);
            RenderWareFileHelper.WriteVector(stream, this.Max);
            RenderWareFileHelper.WriteUint16(stream, this.StartFace);
            RenderWareFileHelper.WriteUint16(stream, this.EndFace);
        }
    }
}
