using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Texture : IBinaryStructure<Texture>
    {
        public ChunkHeader Header { get; set; }
        public ChunkHeader StructHeader { get; set; }
        public byte FilterMode { get; set; }
        public byte UvByte { get; set; }
        public ushort Unknown { get; set; }
        public StringBlock Name { get; set; }
        public StringBlock MaskName { get; set; }
        public Extension Extension { get; set; }

        public uint ContentByteCount => (uint)(4);
        public uint ByteCount => (uint)(ContentByteCount + Extension.Size + 2 * 12 + this.Name.ByteCountWithHeader + this.MaskName.ByteCountWithHeader);
        public uint ByteCountWithHeader => ByteCount + 12;

        public Texture()
        {
            this.Header = new ChunkHeader(6);
            this.StructHeader = new ChunkHeader(1);
            this.FilterMode = 0;
            this.UvByte = 0;
            this.Unknown = 0;
            this.Name = new StringBlock()
            {
                Value = "Texture"
            };
            this.MaskName = new StringBlock()
            {
                Value = "Mask"
            };
            this.Extension = new Extension();
        }

        public Texture Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            this.StructHeader = new ChunkHeader().Read(stream);

            this.FilterMode = RenderWareFileHelper.ReadByte(stream);
            this.UvByte = RenderWareFileHelper.ReadByte(stream);
            this.Unknown = RenderWareFileHelper.ReadUint16(stream);

            this.Name = new StringBlock().Read(stream);
            this.MaskName = new StringBlock().Read(stream);

            this.Extension = new Extension().Read(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            this.StructHeader.Size = ContentByteCount;
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            this.StructHeader.Write(stream);



            RenderWareFileHelper.WriteByte(stream, this.FilterMode);
            RenderWareFileHelper.WriteByte(stream, this.UvByte);
            RenderWareFileHelper.WriteUint16(stream, this.Unknown);

            this.Name.Write(stream);
            this.MaskName.Write(stream);

            this.Extension.Write(stream);
        }
    }
}
