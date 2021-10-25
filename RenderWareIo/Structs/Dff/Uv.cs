using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Uv : IBinaryStructure<Uv>
    {
        public float X { get; set; }
        public float Y { get; set; }


        public uint ContentByteCount => 8;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount;

        public Uv Read(Stream stream)
        {
            this.X = RenderWareFileHelper.ReadFloat(stream);
            this.Y = RenderWareFileHelper.ReadFloat(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteFloat(stream, this.X);
            RenderWareFileHelper.WriteFloat(stream, this.Y);
        }
    }
}
