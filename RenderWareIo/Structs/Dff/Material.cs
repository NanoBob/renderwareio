using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class Material : IBinaryStructure<Material>
    {
        public ChunkHeader Header { get; set; }
        public ChunkHeader StructHeader { get; set; }
        public int Flags { get; set; }
        public Color Color { get; set; }
        public uint Unknown { get; set; }
        public int Textured { get; set; }
        public float Ambient { get; set; }
        public float Specular { get; set; }
        public float Diffuse { get; set; }

        public Texture Texture { get; set; }
        public Extension Extension { get; set; }


        public uint ContentByteCount => (uint)(4 + Color.ByteCountWithHeader + 4 + 4 + 4 + 4 + 4);
        public uint ByteCount => (uint)(ContentByteCount + Extension.Size + 2 * 12 + Texture.ByteCountWithHeader);
        public uint ByteCountWithHeader => ByteCount + 12;

        public Material()
        {
            this.Header = new ChunkHeader(7);
            this.StructHeader = new ChunkHeader(1);
            this.Flags = 0;
            this.Color = new Color();
            this.Unknown = 1;
            this.Textured = 1;
            this.Ambient = 1;
            this.Specular = 1;
            this.Diffuse = 1;
            this.Texture = new Texture();
            this.Extension = new Extension();
        }

        public Material Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            this.StructHeader = new ChunkHeader().Read(stream);

            this.Flags = (int)RenderWareFileHelper.ReadUint32(stream);
            this.Color = new Color().Read(stream);
            this.Unknown = RenderWareFileHelper.ReadUint32(stream);
            this.Textured = (int)RenderWareFileHelper.ReadUint32(stream);

            this.Ambient = RenderWareFileHelper.ReadFloat(stream);
            this.Specular = RenderWareFileHelper.ReadFloat(stream);
            this.Diffuse = RenderWareFileHelper.ReadFloat(stream);

            if (this.Textured > 0)
            {
                this.Texture = new Texture().Read(stream);
            }

            this.Extension = new Extension().Read(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            this.StructHeader.Size = ContentByteCount;
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            this.StructHeader.Write(stream);

            RenderWareFileHelper.WriteUint32(stream, (uint)this.Flags);
            this.Color.Write(stream);
            RenderWareFileHelper.WriteUint32(stream, this.Unknown);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Textured);
            RenderWareFileHelper.WriteFloat(stream, this.Ambient);
            RenderWareFileHelper.WriteFloat(stream, this.Specular);
            RenderWareFileHelper.WriteFloat(stream, this.Diffuse);

            if (this.Textured > 0)
            {
                this.Texture.Write(stream);
            }

            this.Extension.Write(stream);
        }
    }
}
