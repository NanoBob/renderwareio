using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System.IO;

namespace RenderWareIo.Structs.Dff.Plugins
{
    public class FramePlugin : IExtensionPlugin
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

        public string Value { get; set; }

        public uint ContentByteCount => (uint)this.Value.Length;
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount + 12;

        public BinMeshStrip[] BinMeshStrips { get; set; }

        public int Type => 0x0253F2FE;

        public FramePlugin()
        {
            this.Header = new ChunkHeader(0x0253F2FE);
        }

        public void Read(Stream stream)
        {
            this.Header.Read(stream);
            this.Value = new string(RenderWareFileHelper.ReadChars(stream, (int)this.Header.Size));
        }

        public void Write(Stream stream)
        {
            this.Header.Size = (uint)this.Value.Length;
            this.Header.Write(stream);
            RenderWareFileHelper.WriteChars(stream, this.Value.ToCharArray());
        }
    }
}
