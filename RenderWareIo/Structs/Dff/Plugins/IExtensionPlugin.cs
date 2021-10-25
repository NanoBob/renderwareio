using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Dff.Plugins
{
    public interface IExtensionPlugin: IByteCountStructure
    {
        int Type { get; }
        ChunkHeader Header { get; set; }

        void Read(Stream stream);
        void Write(Stream stream);
    }
}
