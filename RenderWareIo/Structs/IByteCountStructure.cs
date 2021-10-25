using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs
{
    public interface IByteCountStructure
    {
        uint ByteCountWithHeader { get; }
        byte[] Bytes { get; }

    }
}
