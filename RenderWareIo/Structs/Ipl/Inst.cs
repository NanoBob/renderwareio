using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RenderWareIo.Structs.Ipl
{
    public struct Inst : IIplEntity<Inst>
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public int Interior { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public int Lod { get; set; }

        public Inst Read(string line)
        {
            string[] splits = line.Split(',').Select((split) => split.Trim()).ToArray();

            this.Id = int.Parse(splits[0]);
            this.ModelName = splits[1];
            this.Interior = int.Parse(splits[2]);
            this.Position = new Vector3(
                float.Parse(splits[3], CultureInfo.InvariantCulture),
                float.Parse(splits[4], CultureInfo.InvariantCulture),
                float.Parse(splits[5], CultureInfo.InvariantCulture)
            );
            this.Rotation = new Quaternion(
                float.Parse(splits[6], CultureInfo.InvariantCulture),
                float.Parse(splits[7], CultureInfo.InvariantCulture),
                float.Parse(splits[8], CultureInfo.InvariantCulture),
                float.Parse(splits[9], CultureInfo.InvariantCulture)
            );
            this.Lod = int.Parse(splits[10]);

            return this;
        }

        public string Write()
        {
            return $"{Id},{ModelName},{Interior},{Position.X},{Position.Y},{Position.Z},{Rotation.X},{Rotation.Y},{Rotation.Z},{Rotation.W},{Lod}";
        }
    }
}
