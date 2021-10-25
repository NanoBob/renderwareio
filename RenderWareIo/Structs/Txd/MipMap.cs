using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Txd
{
    public class MipMap : IBinaryStructure<MipMap>
    {
        public uint Size { get; set; }
        public byte[] Data { get; set; }

        public uint ContentByteCount => (uint)(4 + Data.Length);
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount;



        public MipMap Read(Stream stream)
        {
            this.Size = RenderWareFileHelper.ReadUint32(stream);
            this.Data = new byte[this.Size];
            for (int i = 0; i < this.Data.Length; i++)
            {
                this.Data[i] = RenderWareFileHelper.ReadByte(stream);
            }
            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Data.Length);
            foreach (byte dataByte in this.Data)
            {
                RenderWareFileHelper.WriteByte(stream, dataByte);
            }
        }
    }
}
