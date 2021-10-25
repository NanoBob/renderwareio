using RenderWareIo.ReadWriteHelpers;
using RenderWareIo.Structs.Ipl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.BinaryIpl
{
    public struct BinaryInst :  IBinaryStructure<BinaryInst>
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public int Interior { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public int Lod { get; set; }

        public BinaryInst Read(Stream stream)
        {
            this.ModelName = "N/A";

            this.Position = RenderWareFileHelper.ReadVector(stream);
            this.Rotation = new Quaternion(
                RenderWareFileHelper.ReadFloat(stream),
                RenderWareFileHelper.ReadFloat(stream),
                RenderWareFileHelper.ReadFloat(stream),
                RenderWareFileHelper.ReadFloat(stream)
            );
            this.Id = (int)RenderWareFileHelper.ReadUint32(stream);
            this.Interior = (int)RenderWareFileHelper.ReadUint32(stream);
            this.Lod = (int)RenderWareFileHelper.ReadUint32(stream);

            return this;
        }

        public void Write(Stream stream)
        {
            RenderWareFileHelper.WriteVector(stream, this.Position);
            RenderWareFileHelper.WriteFloat(stream, this.Rotation.X);
            RenderWareFileHelper.WriteFloat(stream, this.Rotation.Y);
            RenderWareFileHelper.WriteFloat(stream, this.Rotation.Z);
            RenderWareFileHelper.WriteFloat(stream, this.Rotation.W);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Id);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Interior);
            RenderWareFileHelper.WriteUint32(stream, (uint)this.Lod);
        }
    }
}
