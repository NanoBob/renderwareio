using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Ide
{
    public struct Anim : IIdeEntity<Anim>
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string TxdName { get; set; }
        public string AnimName { get; set; }
        public int MeshCount { get; set; }
        public float DrawDistance { get; set; }
        public int Flags { get; set; }

        public Anim Read(string line)
        {
            string[] splits = line.Split(',').Select((split) => split.Trim()).ToArray();

            this.Id = int.Parse(splits[0]);
            this.ModelName = splits[1];
            this.TxdName = splits[2];
            this.AnimName = splits[3];
            this.DrawDistance = float.Parse(splits[4]);
            this.Flags = int.Parse(splits[5]);

            return this;
        }

        public string Write()
        {
            throw new NotImplementedException();
        }
    }
}
