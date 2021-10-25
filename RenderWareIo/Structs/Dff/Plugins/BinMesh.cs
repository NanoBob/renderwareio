using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

public struct BinMeshStrip
{
    public uint IndexCount { get; set; }
    public uint MaterialIndex { get; set; }
    public uint[] Indices { get; set; }
}

namespace RenderWareIo.Structs.Dff.Plugins
{
    public class BinMesh : IExtensionPlugin
    {
        public ChunkHeader Header { get; set; }

        public byte[] Bytes
        {
            get
            {
                var stream = new MemoryStream();
                this.Write(stream);
                return stream.ToArray();
            }
        }

        public uint Flags { get; set; }
        public uint MeshCount { get; set; }
        public uint TotalIndexCount { get; set; }

        public uint ContentByteCount => (uint)0;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount + 12;

        public BinMeshStrip[] BinMeshStrips { get; set; }

        public int Type => 0x050e;

        public BinMesh()
        {
            this.Header = new ChunkHeader(0x050e);
        }

        public void Read(Stream stream)
        {
            this.Header.Read(stream);
            this.Flags = RenderWareFileHelper.ReadUint32(stream);
            this.MeshCount = RenderWareFileHelper.ReadUint32(stream);
            this.TotalIndexCount = RenderWareFileHelper.ReadUint32(stream);

            this.BinMeshStrips = new BinMeshStrip[this.MeshCount];
            for (int i = 0; i < this.MeshCount; i++)
            {
                uint indexCount = RenderWareFileHelper.ReadUint32(stream);
                uint materialIndex = RenderWareFileHelper.ReadUint32(stream);
                uint[] indices = new uint[indexCount];

                for (int j = 0; j < indexCount; j++)
                {
                    indices[j] = RenderWareFileHelper.ReadUint32(stream);
                }

                this.BinMeshStrips[i] = new BinMeshStrip()
                {
                    IndexCount = indexCount,
                    MaterialIndex = materialIndex,
                    Indices = indices
                };
            }
        }

        public void Write(Stream stream)
        {

        }
    }
}
