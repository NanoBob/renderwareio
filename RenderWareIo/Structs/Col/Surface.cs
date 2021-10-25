using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Surface : IBinaryStructure<Surface>
    {
        public static int Size => 4;

        public byte Material { get; set; }
        public byte Flag { get; set; }
        public byte Brightness { get; set; }
        public byte Light { get; set; }

        public Surface Read(Stream stream)
        {
            this.Material = RenderWareFileHelper.ReadByte(stream);
            this.Flag = RenderWareFileHelper.ReadByte(stream);
            this.Brightness = RenderWareFileHelper.ReadByte(stream);
            this.Light = RenderWareFileHelper.ReadByte(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteByte(stream, this.Material);
            RenderWareFileHelper.WriteByte(stream, this.Flag);
            RenderWareFileHelper.WriteByte(stream, this.Brightness);
            RenderWareFileHelper.WriteByte(stream, this.Light);
        }
    }
}
