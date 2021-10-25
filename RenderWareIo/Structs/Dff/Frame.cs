using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Frame : IBinaryStructure<Frame>
    {
        public Vector3 Rot1 { get; set; }
        public Vector3 Rot2 { get; set; }
        public Vector3 Rot3 { get; set; }
        public Vector3 Position { get; set; }
        public uint Parent { get; set; }
        public uint Flags { get; set; }


        public uint ContentByteCount => 56;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount;

        public Frame()
        {
            this.Rot1 = Vector3.Zero;
            this.Rot2 = Vector3.Zero;
            this.Rot3 = Vector3.Zero;
            this.Position = Vector3.Zero;
            this.Parent = 4294967295;
            this.Flags = 0;
        }

        public Frame Read(Stream stream)
        {
            this.Rot1 = RenderWareFileHelper.ReadVector(stream);
            this.Rot2 = RenderWareFileHelper.ReadVector(stream);
            this.Rot3 = RenderWareFileHelper.ReadVector(stream);
            this.Position = RenderWareFileHelper.ReadVector(stream);
            this.Parent = RenderWareFileHelper.ReadUint32(stream);
            this.Flags = RenderWareFileHelper.ReadUint32(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteVector(stream, this.Rot1);
            RenderWareFileHelper.WriteVector(stream, this.Rot2);
            RenderWareFileHelper.WriteVector(stream, this.Rot3);
            RenderWareFileHelper.WriteVector(stream, this.Position);
            RenderWareFileHelper.WriteUint32(stream, this.Parent);
            RenderWareFileHelper.WriteUint32(stream, this.Flags);
        }
    }
}
