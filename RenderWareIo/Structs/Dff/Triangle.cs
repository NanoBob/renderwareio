using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Triangle : IBinaryStructure<Triangle>
    {
        public ushort VertexIndexOne { get; set; }
        public ushort VertexIndexTwo { get; set; }
        public ushort VertexIndexThree { get; set; }
        public ushort MaterialIndex { get; set; }


        public uint ContentByteCount => 8;
        public uint ByteCount => 8;
        public uint ByteCountWithHeader => 8;

        public Triangle()
        {
            this.VertexIndexOne = 0;
            this.VertexIndexTwo = 0;
            this.VertexIndexThree = 0;
            this.MaterialIndex = 0;
        }

        public Triangle Read(Stream stream)
        {
            this.VertexIndexOne = RenderWareFileHelper.ReadUint16(stream);
            this.VertexIndexTwo = RenderWareFileHelper.ReadUint16(stream);
            this.MaterialIndex = RenderWareFileHelper.ReadUint16(stream);
            this.VertexIndexThree = RenderWareFileHelper.ReadUint16(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteUint16(stream, this.VertexIndexOne);
            RenderWareFileHelper.WriteUint16(stream, this.VertexIndexTwo);
            RenderWareFileHelper.WriteUint16(stream, this.MaterialIndex);
            RenderWareFileHelper.WriteUint16(stream, this.VertexIndexThree);
        }
    }
}
