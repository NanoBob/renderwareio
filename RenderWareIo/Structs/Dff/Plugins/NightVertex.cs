using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.IO;
using System.Linq;
using SystemColor = System.Drawing.Color;

namespace RenderWareIo.Structs.Dff.Plugins
{
    public class NightVertex : IExtensionPlugin
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

        public uint ContentByteCount => (uint)(Colors.Length * 4) + 4;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount + 12;

        public SystemColor[] Colors { get; set; }

        public int Type => 0x253F2F9;

        public NightVertex(SystemColor[] colors)
        {
            Colors = colors;
            this.Header = new ChunkHeader(0x253F2F9);
            this.Header.Size = ContentByteCount;
        }

        public void Read(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Write(Stream stream)
        {
            this.Header.Write(stream);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Colors.Length);
            var bytes = this.Colors.SelectMany(x => new byte[] { x.R, x.G, x.B, x.A });
            foreach (var @byte in bytes)
            {
                RenderWareFileHelper.WriteByte(stream, @byte);
            }
        }
    }
}
