using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs.Col
{
    public class Col : IBinaryStructure<Col>
    {
        public Header Header { get; set; }
        public Body Body { get; set; }

        public Col()
        {
            this.Header = new Header();
            this.Body = new Body();
        }

        public Col Read(Stream stream)
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
}
