using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    public class MaterialList : IBinaryStructure<MaterialList>
    {
        public ChunkHeader Header { get; set; }
        public ChunkHeader StructHeader { get; set; }
        public int MaterialCount { get; set; }
        public int Unknown { get; set; } // apparently material indices??
        public List<Material> Materials { get; set; }


        public uint ContentByteCount => (uint)(4 + this.Materials.Count * 4);
        public uint ByteCount => (uint)(ContentByteCount + 12 + Materials.Sum(material => material.ByteCountWithHeader));
        public uint ByteCountWithHeader => ByteCount + 12;

        public MaterialList()
        {
            this.Header = new ChunkHeader(8);
            this.StructHeader = new ChunkHeader(1);
            this.MaterialCount = 0;
            this.Unknown = -1;
            this.Materials = new List<Material>();
        }

        public MaterialList Read(Stream stream)
        {
            this.Header = new ChunkHeader().Read(stream);
            this.StructHeader = new ChunkHeader().Read(stream);
            this.MaterialCount = (int)RenderWareFileHelper.ReadUint32(stream);

            if (this.MaterialCount > 0)
            {
                for (int i = 0; i < this.MaterialCount; i++)
                {
                    this.Unknown = (int)RenderWareFileHelper.ReadUint32(stream);
                }
                //this.Unknown = RenderWareFileHelper.ReadUint32(stream);
            }

            this.Materials = RenderWareFileHelper.ReadBinaryStructure<Material>(stream, this.MaterialCount);
            
            return this;
        }

        public void Write(Stream stream)
        {
            this.StructHeader.Size = ContentByteCount;
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            this.StructHeader.Write(stream);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Materials.Count);

            if (this.Materials.Count > 0)
            {
                for (int i = 0; i < this.Materials.Count; i++)
                {
                    RenderWareFileHelper.WriteUint32(stream, (uint)this.Unknown);
                }
            }

            RenderWareFileHelper.WriteBinaryStructure(stream, this.Materials);
        }
    }
}
