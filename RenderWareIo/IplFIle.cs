using RenderWareIo.Structs.Ipl;
using RenderWareIo.Structs.Txd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo
{
    public class IplFile
    {
        public Ipl Ipl { get; set; }


        public IplFile()
        {
            this.Ipl = new Ipl();
        }

        public IplFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Ipl file '{path}' does not exist");
            }

            string data = File.ReadAllText(path);

            this.Ipl = Ipl.Read(data);
        }

        public void Write(string path)
        {
            File.WriteAllText(path, this.Ipl.Write());
        }
    }
}
