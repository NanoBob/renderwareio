using RenderWareIo.ReadWriteHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.BinaryIpl
{
    public struct BinaryIpl : IBinaryStructure<BinaryIpl>
    {
        public IplHeader Header { get; set; }
        public List<BinaryInst> Insts { get; set; }

        public BinaryIpl Read(Stream stream)
        {
            this.Header = new IplHeader().Read(stream);
            this.Insts = RenderWareFileHelper.ReadBinaryStructure<BinaryInst>(stream, (int)this.Header.InstanceCount);

            return this;
        }


        public void Write(Stream stream)
        {
            this.Header.Write(stream);
            RenderWareFileHelper.WriteBinaryStructure(stream, this.Insts);
        }
    }
}
