using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Vertex : IBinaryStructure<Vertex>
    {
        public uint Size => 6;

        public short First { get; set; }
        public float FirstFloat
        {
            get => First / 128.0f;
            set { First = (short)(value * 128); }
        }


        public short Second { get; set; }
        public float SecondFloat
        {
            get => Second / 128.0f;
            set { Second = (short)(value * 128); }
        }

        public short Third { get; set; }
        public float ThirdFloat
        {
            get => Third / 128.0f;
            set { Third = (short)(value * 128); }
        }

        public Vertex Read(Stream stream)
        {
            this.First = (short)RenderWareFileHelper.ReadUint16(stream);
            this.Second = (short)RenderWareFileHelper.ReadUint16(stream);
            this.Third = (short)RenderWareFileHelper.ReadUint16(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteUint16(stream, (ushort)this.First);
            RenderWareFileHelper.WriteUint16(stream, (ushort)this.Second);
            RenderWareFileHelper.WriteUint16(stream, (ushort)this.Third);
        }
    }
}
