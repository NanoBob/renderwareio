using RenderWareIo.Structs.Dat;
using RenderWareIo.Structs.Txd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo
{
    public class DatFile
    {
        public Dat Dat { get; set; }


        public DatFile()
        {
            this.Dat = new Dat();
        }

        public DatFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Dat file '{path}' does not exist");
            }

            string data = File.ReadAllText(path);

            this.Dat = Dat.Read(data);
        }

        public void Write(string path)
        {
            File.WriteAllText(path, this.Dat.Write());
        }
    }
}
