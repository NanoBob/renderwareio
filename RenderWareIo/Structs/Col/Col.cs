using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class ColCombo : IBinaryStructure<ColCombo>
    {
        public Header Header { get; set; }
        public Body Body { get; set; }

        public ColCombo()
        {
            this.Header = new Header();
            this.Body = new Body();
        }

        public ColCombo Read(Stream stream)
        {
            this.Header = Header.Read(stream);
            this.Body = Body.Read(stream, this.Header);
            return this;
        }


        public void Write(Stream stream)
        {
            this.Header.Generate(this.Body);

            this.Header.Write(stream);
            this.Body.Write(stream);
        }
    }

    public class Col : IBinaryStructure<Col>
    {
        private static byte[] Col3 { get; } = new byte[] { 67, 79, 76, 51 };

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
                if (stream.Position < expectedStop)
                {
                    stream.Position = expectedStop;
                }

                var nextBytes = new byte[4];
                stream.Read(nextBytes, 0, 4);
                stream.Position -= 4;
                if (!nextBytes.SequenceEqual(Col3))
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
