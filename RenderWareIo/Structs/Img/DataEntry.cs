using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Img
{
    public class DataEntry 
    {
        const int sectorSize = 2048;

        private readonly Stream stream;
        private readonly DirectoryEntry entry;

        public byte[] Data
        {
            get
            {
                int byteCount = entry.StreamingSize * sectorSize;
                byte[] buffer = new byte[byteCount];

                long oldPosition = stream.Position;

                int offset = (int)entry.Offset * sectorSize;
                stream.Position = offset;
                stream.Read(buffer, 0, byteCount);

                stream.Position = oldPosition;
                return buffer;
            }
        }

        public DataEntry(Stream stream, DirectoryEntry entry)
        {
            this.stream = stream;
            this.entry = entry;
        }
    }
}
