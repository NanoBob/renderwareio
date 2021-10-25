using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.BinaryIpl
{
    public struct IplHeader : IBinaryStructure<IplHeader>
    {
        public char[] Type { get; set; }
        public uint[] FullHeader { get; set; }
        public uint InstanceCount => FullHeader[0];
        public uint CarCount => FullHeader[4];
        public uint ItemOffset => FullHeader[6];


        public IplHeader Read(Stream stream)
        {
            this.Type = RenderWareFileHelper.ReadChars(stream, 4);
            this.FullHeader = new uint[18];
            for (int i = 0; i < 18; i++)
            {
                FullHeader[i] = RenderWareFileHelper.ReadUint32(stream);
            }

            return this;
        }


        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteChars(stream, this.Type);
            foreach (byte b in this.FullHeader)
            {
                RenderWareFileHelper.WriteByte(stream, b);
            }
        }
    }
}
