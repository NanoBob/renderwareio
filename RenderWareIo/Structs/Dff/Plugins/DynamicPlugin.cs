using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using RenderWareIo.Structs.Dff.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Dff.Plugins
{
    public class DynamicPlugin : IExtensionPlugin
    {
        public uint ByteCountWithHeader => throw new NotImplementedException();

        public byte[] Bytes => throw new NotImplementedException();

        public int Type => throw new NotImplementedException();

        public ChunkHeader Header { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Read(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Write(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
