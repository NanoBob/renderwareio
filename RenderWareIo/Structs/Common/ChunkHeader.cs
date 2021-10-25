using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Common
{
    public class ChunkHeader : IBinaryStructure<ChunkHeader>
    {
        public uint Type { get; set; }
        public uint Size { get; set; }
        public uint Marker { get; set; }

        public ChunkHeader(uint type = 0)
        {
            this.Type = type;
            this.Size = 0;
            this.Marker = 0x1803FFFF;
        }

        public ChunkHeader Read(Stream stream)
        {
            this.Type = RenderWareFileHelper.ReadUint32(stream);
            this.Size = RenderWareFileHelper.ReadUint32(stream);
            this.Marker = RenderWareFileHelper.ReadUint32(stream);

            return this;
        }


        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteUint32(stream, this.Type);
            RenderWareFileHelper.WriteUint32(stream, this.Size);
            RenderWareFileHelper.WriteUint32(stream, this.Marker);
        }
    }
}
