using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Ide
{
    public struct Weapon : IIdeEntity<Weapon>
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string TxdName { get; set; }
        public string UnknownString { get; set; }
        public int UnknownInt1 { get; set; }
        public int UnknownInt2 { get; set; }
        public int UnknownInt3 { get; set; }

        public Weapon Read(string line)
        {
            string[] splits = line.Split(',').Select((split) => split.Trim()).ToArray();

            this.Id = int.Parse(splits[0]);
            this.ModelName = splits[1];
            this.TxdName = splits[2];
            this.UnknownString = splits[3];
            this.UnknownInt1 = int.Parse(splits[4]);
            this.UnknownInt2 = int.Parse(splits[5]);
            this.UnknownInt3 = int.Parse(splits[6]);

            return this;
        }

        public string Write()
        {
            throw new NotImplementedException();
        }
    }
}
