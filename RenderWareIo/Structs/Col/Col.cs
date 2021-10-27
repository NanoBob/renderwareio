using RenderWareIo.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Col : IBinaryStructure<Col>
    {

        public List<ColCombo> ColCombos { get; set; }

        public Col()
        {
            this.ColCombos = new List<ColCombo>();
        }

        public Col Read(Stream stream)
        {
            while (stream.Position < stream.Length)
            {
                var start = stream.Position;
                var colCombo = new ColCombo().Read(stream);
                this.ColCombos.Add(colCombo);

                var expectedStop = start + colCombo.Header.Size + 8;
                if (stream.Position != expectedStop)
                {
                    stream.Position = expectedStop;
                }

                var nextBytes = new byte[4];
                stream.Read(nextBytes, 0, 4);
                stream.Position -= 4;
                if (!(nextBytes.SequenceEqual(ColConstants.Col2) || nextBytes.SequenceEqual(ColConstants.Col3)))
                    break;
            }
            return this;
        }


        public void Write(Stream stream)
        {
            foreach (var combo in this.ColCombos)
                combo.Write(stream);
        }
    }
}
