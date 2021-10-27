using System.IO;

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
            this.Body.Write(stream, this.Header);
        }
    }
}
