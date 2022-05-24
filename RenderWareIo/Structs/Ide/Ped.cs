using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Ide
{
    public struct Ped : IIdeEntity<Ped>
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string TxdName { get; set; }
        public string PedType { get; set; }
        public string AnimGroup { get; set; }
        public string CarMask { get; set; }
        public string Flags { get; set; }
        public string AnimFile { get; set; }
        public string Radio1 { get; set; }
        public string Radio2 { get; set; }

        public Ped Read(string line)
        {
            string[] splits = line.Split(',').Select((split) => split.Trim()).ToArray();

            this.Id = int.Parse(splits[0]);
            this.ModelName = splits[1];
            this.TxdName = splits[2];
            this.PedType = splits[3];
            this.AnimFile = splits[4];
            this.CarMask = splits[5];
            this.Flags = splits[6];
            this.AnimFile = splits[7];
            this.Radio1 = splits[8];
            this.Radio2 = splits[9];

            return this;
        }

        public string Write()
        {
            throw new NotImplementedException();
        }
    }
}
