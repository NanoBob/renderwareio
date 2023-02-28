using System;
using System.Globalization;
using System.Linq;

namespace RenderWareIo.Structs.Ide
{
    public struct Car : IIdeEntity<Car>
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string TxdName { get; set; }
        public string Type { get; set; }
        public string HandlingId { get; set; }
        public string GameName { get; set; }
        public string Anims { get; set; }
        public string Class { get; set; }
        public int Frequency { get; set; }

        public Car Read(string line)
        {
            line = line.Replace(" ", "");

            while (line.IndexOf("\t\t") > 0)
                line = line.Replace("\t\t", "\t");

            string[] splits = line.Split('\t').Select((split) => split.Trim().Trim(',')).ToArray();

            this.Id = int.Parse(splits[0]);
            this.ModelName = splits[1];
            this.TxdName = splits[2];
            this.Type = splits[3];
            this.HandlingId = splits[4];
            this.GameName = splits[5];
            this.Anims = splits[6];
            this.Class = splits[7];
            this.Frequency = int.Parse(splits[8]);

            return this;
        }

        public string Write()
        {
            throw new NotImplementedException();
        }
    }
}
