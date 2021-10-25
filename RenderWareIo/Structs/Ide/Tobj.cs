using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Ide
{
    public struct Tobj : IIdeEntity<Tobj>
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string TxdName { get; set; }
        public int MeshCount { get; set; }
        public float[] DrawDistances { get; set; }
        public int Flags { get; set; }
        public int TimeOn { get; set; }
        public int TimeOff { get; set; }

        public Tobj Read(string line)
        {
            string[] splits = line.Split(',').Select((split) => split.Trim()).ToArray();

            this.Id = int.Parse(splits[0]);
            this.ModelName = splits[1];
            this.TxdName = splits[2];

            if (splits.Length > 7)
            {
                this.MeshCount = int.Parse(splits[3]);

                this.DrawDistances = new float[splits.Length - 4];
                for (int i = 0; i < (splits.Length - 4); i++)
                {
                    this.DrawDistances[i] = float.Parse(splits[4 + i]);
                }

            } else
            {
                this.MeshCount = 1;
                this.DrawDistances = new float[1] { float.Parse(splits[4]) };
            }
            this.Flags = int.Parse(splits[splits.Length - 3]);
            this.TimeOn = int.Parse(splits[splits.Length - 2]);
            this.TimeOff = int.Parse(splits[splits.Length - 1]);

            return this;
        }

        public string Write()
        {
            throw new NotImplementedException();
        }
    }
}
