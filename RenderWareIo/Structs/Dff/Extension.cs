using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using RenderWareIo.Structs.Dff.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RenderWareIo.Structs.Dff
{
    public class Extension : IBinaryStructure<Extension>
    {
        public static Dictionary<uint, Func<Stream, IExtensionPlugin>> ExtensionTypes = new Dictionary<uint, Func<Stream, IExtensionPlugin>>()
        {
            [0x050E] = (stream) =>
            {
                var extension = new BinMesh();
                extension.Read(stream);
                return extension;
            },
            [0x0253F2FE] = (stream) =>
            {
                var extension = new FramePlugin();
                extension.Read(stream);
                return extension;
            },
        };

        public ChunkHeader Header { get; set; }
        public List<byte> Data { get; set; }
        public int Size => Data.Count;
        public List<IExtensionPlugin> Extensions { get; set; }


        public uint ContentByteCount => (uint)(Size + Extensions.Sum(x => x.ByteCountWithHeader));
        public uint ByteCount => ContentByteCount;
        public uint ByteCountWithHeader => ByteCount + 12;

        public Extension()
        {
            this.Header = new ChunkHeader(3);
            this.Data = new List<byte>();
            this.Extensions = new List<IExtensionPlugin>();
        }

        public Extension Read(Stream stream)
        {
            if (stream.Position == stream.Length || stream.Position > stream.Length - 12)
            {
                this.Header = new ChunkHeader(3);
                this.Data = new List<byte>();
                Console.WriteLine("Encountered stream end instead of dff extension");
                return this;
            }

            this.Header = new ChunkHeader().Read(stream);

            if (Header.Type != 3)
            {
                if (Header.Type == 1)
                {
                    stream.Position -= 12;
                }
                //throw new Exception($"Encountered header type {Header.Type} instead of 3 at position {stream.Position - 12}");
                return this;
            }

            this.Data = new List<byte>();
            uint lengthLeft = this.Header.Size;
            while (lengthLeft > 0)
            {
                ChunkHeader header = new ChunkHeader().Read(stream);

                if (ExtensionTypes.ContainsKey(header.Type))
                {
                    stream.Position -= 12;
                    var plugin = ExtensionTypes[header.Type](stream);
                    this.Extensions.Add(plugin);
                }
                else
                {
                    stream.Position -= 12;
                    for (int i = 0; i < header.Size + 12; i++)
                        this.Data.Add(RenderWareFileHelper.ReadByte(stream));
                }
                lengthLeft -= (header.Size + 12);
            }

            return this;
        }

        public void Write(Stream stream)
        {
            this.Header.Size = ByteCount;

            this.Header.Write(stream);
            foreach (IExtensionPlugin extensionPlugin in this.Extensions)
            {
                extensionPlugin.Write(stream);
            }

            foreach (byte dataByte in this.Data)
            {
                RenderWareFileHelper.WriteByte(stream, dataByte);
            }
        }

        public static Extension Empty()
        {
            return new Extension()
            {
                Header = new ChunkHeader(3),
                Data = new List<byte>()
            };
        }
    }
}
