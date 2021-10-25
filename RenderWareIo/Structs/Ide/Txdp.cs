using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Ide
{
    public struct Txdp : IIdeEntity<Txdp>
    {
        public string SourceTxd { get; set; }
        public string TargetTxd { get; set; }

        public Txdp Read(string line)
        {
            string[] splits = line.Split(',').Select((split) => split.Trim()).ToArray();

            this.SourceTxd = splits[0];
            this.TargetTxd = splits[1];

            return this;
        }

        public string Write()
        {
            throw new NotImplementedException();
        }
    }
}
