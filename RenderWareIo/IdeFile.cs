using RenderWareIo.Structs.Ide;
using RenderWareIo.Structs.Txd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo
{
    public class IdeFile
    {
        public Ide Ide { get; set; }


        public IdeFile()
        {
            this.Ide = new Ide();
        }

        public IdeFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Ide file '{path}' does not exist");
            }

            string data = File.ReadAllText(path);

            this.Ide = Ide.Read(data);
        }

        public void Write(string path)
        {
            File.WriteAllText(path, this.Ide.Write());
        }
    }
}
