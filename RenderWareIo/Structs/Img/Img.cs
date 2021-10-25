using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Img
{
    public struct Img : IBinaryStructure<Img>
    {
        public char[] Version { get; set; }
        public uint ItemCount { get; set; }
        public List<DirectoryEntry> DirectoryEntries { get; set; }
        public Dictionary<string, DataEntry> DataEntries { get; set; }
        public List<string> IplFiles { get; set; }

        public Img Read(Stream stream)
        {
            this.IplFiles = new List<string>();

            this.Version = RenderWareFileHelper.ReadChars(stream, 4);
            this.ItemCount = RenderWareFileHelper.ReadUint32(stream);
            this.DirectoryEntries = RenderWareFileHelper.ReadBinaryStructure<DirectoryEntry>(stream, (int)this.ItemCount);

            this.DataEntries = new Dictionary<string, DataEntry>();
            foreach(DirectoryEntry directoryEntry in this.DirectoryEntries)
            {
                string sanitizedKey = directoryEntry.Name.Trim('\0').ToLower();
                this.DataEntries[sanitizedKey] = new DataEntry(stream, directoryEntry);

                if (sanitizedKey.EndsWith(".ipl"))
                {
                    this.IplFiles.Add(sanitizedKey);
                }
            }

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteChars(stream, this.Version);
            RenderWareFileHelper.WriteUint32(stream, this.ItemCount);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.DirectoryEntries);
        }
    }
}
