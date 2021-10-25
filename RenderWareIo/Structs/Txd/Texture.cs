using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Txd
{
    public class Texture : IBinaryStructure<Texture>
    {
        public ChunkHeader Header { get; set; }
        public TextureData Data { get; set; }
        public TextureExtraInfo ExtraInfo { get; set; }

        public uint ContentByteCount => Data.ByteCountWithHeader;
        public uint ByteCount => (uint)(ContentByteCount + ExtraInfo.Data.Length + 12);
        public uint ByteCountWithHeader => ByteCount + 12;

        public Texture()
        {
            this.Header = new ChunkHeader(21);
            this.Data = new TextureData();
            this.ExtraInfo = new TextureExtraInfo();
        }

        public Texture Read(Stream stream)
        {
            try
            {
                this.Header = new ChunkHeader().Read(stream);
                this.Data = new TextureData().Read(stream);

                if (stream.Position == stream.Length)
                {
                    Console.WriteLine("Encountered stream end instead of texture extra info");
                    this.ExtraInfo = TextureExtraInfo.Empty();
                    return this;
                }

                var header = new ChunkHeader().Read(stream);
                stream.Position -= 12;

                if (header.Type != 3)
                {
                    Console.WriteLine($"Encountered header {header.Type} instead of texture extra info (3)");
                    this.ExtraInfo = TextureExtraInfo.Empty();
                    return this;
                }
                this.ExtraInfo = new TextureExtraInfo().Read(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read texture");
            }

            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            this.Data.Write(stream);
            this.ExtraInfo.Write(stream);
        }

        public byte[] GetDds(bool withMipMaps = false)
        {
            byte[][] mipmaps = null;
            if (withMipMaps)
            {
                mipmaps = new byte[this.Data.MipMapCount - 1][];
                for (int i = 0; i < this.Data.MipMapCount - 1; i++)
                {
                    mipmaps[i] = this.Data.MipMaps[i].Data;
                }
            }
            return DdsHelper.GetDdsBytes(this.Data.Data, this.Data.TextureFormat, this.Data.Width, this.Data.Height, mipmaps);
        }
    }
}
