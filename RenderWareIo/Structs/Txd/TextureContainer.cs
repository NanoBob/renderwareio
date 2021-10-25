using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Txd
{
    public class TextureContainer : IBinaryStructure<TextureContainer>
    {
        public ChunkHeader Header { get; set; }
        public ushort Count { get; set; }
        public ushort Unknown { get; set; }
        public List<Texture> Textures { get; set; }
        public TextureExtraInfo ExtraInfo { get; set; }

        public uint ContentByteCount => (uint)(2 + 2);
        public uint ByteCount => (uint)(ContentByteCount);
        public uint ByteCountWithHeader => (uint)(ByteCount + 12 + Textures.Sum(t => t.ByteCountWithHeader));

        public TextureContainer()
        {
            this.Header = new ChunkHeader(1);
            this.Unknown = 0;
            this.Textures = new List<Texture>();
            this.ExtraInfo = new TextureExtraInfo();
        }

        public TextureContainer Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            this.Count = RenderWareFileHelper.ReadUint16(stream);
            this.Unknown = RenderWareFileHelper.ReadUint16(stream);
            this.Textures = RenderWareFileHelper.ReadBinaryStructure<Texture>(stream, this.Count);
            this.ExtraInfo = new TextureExtraInfo().Read(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            RenderWareFileHelper.WriteUint16(stream, (ushort)this.Textures.Count);
            RenderWareFileHelper.WriteUint16(stream, this.Unknown);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.Textures);
            this.ExtraInfo.Write(stream);
        }
    }
}
