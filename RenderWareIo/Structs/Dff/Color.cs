using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Color : IBinaryStructure<Color>
    {

        public uint ContentByteCount => 4;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount;

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color()
        {
            this.R = 255;
            this.G = 255;
            this.B = 255;
            this.A = 255;
        }

        public Color Read(Stream stream)
        {
            this.R = RenderWareFileHelper.ReadByte(stream);
            this.G = RenderWareFileHelper.ReadByte(stream);
            this.B = RenderWareFileHelper.ReadByte(stream);
            this.A = RenderWareFileHelper.ReadByte(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteByte(stream, this.R);
            RenderWareFileHelper.WriteByte(stream, this.G);
            RenderWareFileHelper.WriteByte(stream, this.B);
            RenderWareFileHelper.WriteByte(stream, this.A);
        }
    }
}
