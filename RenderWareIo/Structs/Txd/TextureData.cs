using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Txd
{
    public class TextureDataLookup
    {
        public static Dictionary<uint, string> TextureFormats = new Dictionary<uint, string>()
        {
            [0x00] = "S3TC DXT1",
            [22] = "RGB32",
            [21] = "RGBA32",
        };
    }

    public class TextureData : IBinaryStructure<TextureData>
    {
        public ChunkHeader Header { get; set; }
        public uint Version { get; set; }
        public uint FilterFlags { get; set; }
        public string TextureName { get; set; }
        public string AlphaName { get; set; }
        public uint AlphaFlags { get; set; }
        public uint TextureFormat { get; set; }
        public string TextureFormatString
        {
            get =>
                TextureDataLookup.TextureFormats.ContainsKey(TextureFormat) ?
                TextureDataLookup.TextureFormats[TextureFormat] :
                string.Join("", BitConverter.GetBytes(TextureFormat).Select((ccByte) => (char)ccByte));
            set 
            {
                TextureFormat = BitConverter.ToUInt32(value.Select(character => (byte)character).ToArray(), 0);
            }
        }

        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public byte Depth { get; set; }
        public byte MipMapCount { get; set; }
        public byte TexCodeType { get; set; }
        public byte Flags { get; set; }
        public byte[] Pallette { get; set; }
        public uint DataSize { get; set; }
        public byte[] Data { get; set; }
        public List<MipMap> MipMaps { get; set; }


        public uint ContentByteCount => (uint)(
            4 + 4 + 32 + 32 + 
            4 + 4 + 2 + 2 + 1 + 1 + 1 + 1 + 
            (this.Depth == 7 ? 256 * 4 : 0) +
            4 + this.Data.Length +
            this.MipMaps.Sum(mipmap => mipmap.ByteCountWithHeader));
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount + 12;

        public TextureData()
        {
            this.Header = new ChunkHeader(1);
            this.Version = 0x09;
            this.FilterFlags = 0x1106;
            this.TextureName = "TextureName";
            this.AlphaName = "";
            this.AlphaFlags = 0x8200;
            this.TextureFormatString = "DXT1";
            this.Depth = 16;
            this.TexCodeType = 4;
            this.Flags = 8;
            this.MipMaps = new List<MipMap>();
            this.Pallette = new byte[0];
        }

        public TextureData Read(Stream stream)
        {
            try
            {
                this.Header = new ChunkHeader().Read(stream);
                this.Version = RenderWareFileHelper.ReadUint32(stream);
                this.FilterFlags = RenderWareFileHelper.ReadUint32(stream);
                this.TextureName = string.Join("", RenderWareFileHelper.ReadChars(stream, 32));
                this.AlphaName = string.Join("", RenderWareFileHelper.ReadChars(stream, 32));

                this.AlphaFlags = RenderWareFileHelper.ReadUint32(stream);
                this.TextureFormat = RenderWareFileHelper.ReadUint32(stream);
                this.Width = RenderWareFileHelper.ReadUint16(stream);
                this.Height = RenderWareFileHelper.ReadUint16(stream);
                this.Depth = RenderWareFileHelper.ReadByte(stream);
                this.MipMapCount = RenderWareFileHelper.ReadByte(stream);
                this.TexCodeType = RenderWareFileHelper.ReadByte(stream);
                this.Flags = RenderWareFileHelper.ReadByte(stream);

                this.Pallette = new byte[this.Depth == 7 ? 256 * 4 : 0];
                for (int i = 0; i < this.Pallette.Length; i++)
                {
                    this.Pallette[i] = RenderWareFileHelper.ReadByte(stream);
                }
                this.DataSize = RenderWareFileHelper.ReadUint32(stream);
                this.Data = new byte[this.DataSize];
                stream.Read(this.Data, 0, (int)this.DataSize);
                //for (int i = 0; i < this.Data.Length; i++)
                //{
                //    this.Data[i] = RenderWareFileHelper.ReadByte(stream);
                //}
                this.MipMaps = RenderWareFileHelper.ReadBinaryStructure<MipMap>(stream, this.MipMapCount - 1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read texture data");
            }

            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Size = ByteCount;

            this.Header.Write(stream);

            RenderWareFileHelper.WriteUint32(stream, this.Version);
            RenderWareFileHelper.WriteUint32(stream, this.FilterFlags);
            RenderWareFileHelper.WriteChars(stream, this.TextureName.PadRight(32, '\0').ToCharArray());
            RenderWareFileHelper.WriteChars(stream, this.AlphaName.PadRight(32, '\0').ToCharArray());

            RenderWareFileHelper.WriteUint32(stream, this.AlphaFlags);
            RenderWareFileHelper.WriteUint32(stream, this.TextureFormat);
            RenderWareFileHelper.WriteUint16(stream, this.Width);
            RenderWareFileHelper.WriteUint16(stream, this.Height);
            RenderWareFileHelper.WriteByte(stream, this.Depth);
            RenderWareFileHelper.WriteByte(stream, (byte)(this.MipMaps.Count + 1));
            RenderWareFileHelper.WriteByte(stream, this.TexCodeType);
            RenderWareFileHelper.WriteByte(stream, this.Flags);

            foreach(byte palletteByte in this.Pallette)
            {
                RenderWareFileHelper.WriteByte(stream, palletteByte);
            }

            RenderWareFileHelper.WriteUint32(stream, (uint)this.Data.Length);
            //foreach (byte dataByte in this.Data)
            //{
            //    RenderWareFileHelper.WriteByte(stream, dataByte);
            //}
            stream.Write(this.Data, 0, this.Data.Length);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.MipMaps);

        }

        public byte[] GetDds(bool withMipMaps = false)
        {
            byte[][] mipmaps = null;
            if (withMipMaps)
            {
                mipmaps = new byte[this.MipMapCount - 1][];
                for (int i = 0; i < this.MipMapCount - 1; i++)
                {
                    mipmaps[i] = this.MipMaps[i].Data;
                }
            }
            return DdsHelper.GetDdsBytes(this.Data, this.TextureFormat, this.Width, this.Height, mipmaps);
        }

        public void SetDds(byte[] dds, bool withMipMaps = false)
        {

            var strippedDds = DdsHelper.StripDdsHeader(dds, out uint fourCC, out uint width, out uint height);

            int mainSize = (int)(width * height / 2);
            var mainData = strippedDds.Take(mainSize);

            this.Data = mainData.ToArray();
            this.TextureFormat = fourCC;
            this.Width = (ushort)width;
            this.Height = (ushort)height;

            if (withMipMaps)
            {
                int offset = mainSize;
                int previousSize = mainSize;

                while (offset < strippedDds.Length)
                {
                    int mipmapSize = previousSize;
                    int remainder = strippedDds.Length - offset;
                    if (remainder < previousSize)
                    {
                        mipmapSize = previousSize / 4;
                    }

                    var mipmap = strippedDds.Skip(offset).Take(mipmapSize).ToArray();
                    this.MipMaps.Add(new MipMap()
                    {
                        Data = mipmap
                    });

                    offset += mipmapSize;
                    previousSize = mipmapSize;
                }
            }
        }
    }
}
