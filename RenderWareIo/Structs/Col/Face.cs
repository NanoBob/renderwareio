using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Face : IBinaryStructure<Face>
    {
        public uint Size => 8;

        public ushort A { get; set; }
        public ushort B { get; set; }
        public ushort C { get; set; }
        public byte Material { get; set; }
        public byte Light { get; set; }

        public Face Read(Stream stream)
        {
            this.A = RenderWareFileHelper.ReadUint16(stream);
            this.B = RenderWareFileHelper.ReadUint16(stream);
            this.C = RenderWareFileHelper.ReadUint16(stream);
            this.Material = RenderWareFileHelper.ReadByte(stream);
            this.Light = RenderWareFileHelper.ReadByte(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteUint16(stream, this.A);
            RenderWareFileHelper.WriteUint16(stream, this.B);
            RenderWareFileHelper.WriteUint16(stream, this.C);
            RenderWareFileHelper.WriteByte(stream, this.Material);
            RenderWareFileHelper.WriteByte(stream, this.Light);
        }
    }
}
