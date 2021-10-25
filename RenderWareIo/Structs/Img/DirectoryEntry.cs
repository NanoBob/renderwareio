using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Img
{
    public class DirectoryEntry : IBinaryStructure<DirectoryEntry>
    {
        public uint Offset { get; set; }
        public ushort StreamingSize { get; set; }
        public ushort Size { get; set; }
        public string Name { get; set; } 

        public DirectoryEntry Read(Stream stream)
        {
            this.Offset = RenderWareFileHelper.ReadUint32(stream);
            this.StreamingSize = RenderWareFileHelper.ReadUint16(stream);
            this.Size = RenderWareFileHelper.ReadUint16(stream);
            this.Name = string.Join("", RenderWareFileHelper.ReadChars(stream, 24));
            this.Name = (this.Name.Split('\0')[0]);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteUint32(stream, this.Offset);
            RenderWareFileHelper.WriteUint16(stream, this.StreamingSize);
            RenderWareFileHelper.WriteUint16(stream, this.Size);
            RenderWareFileHelper.WriteChars(stream, this.Name.PadRight(24, '\0').ToCharArray());
        }
    }
}
