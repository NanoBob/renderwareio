using RenderWareIo.Structs.Txd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo
{
    public class TxdFile
    {
        Stream stream;

        public Txd Txd { get; set; }


        public TxdFile()
        {
            this.Txd = new Txd();
        }

        public TxdFile(byte[] data)
        {
            this.stream = new MemoryStream(data);

            this.Txd = Txd.Read(this.stream);
            this.stream.Close();
        }

        public TxdFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Txd file '{path}' does not exist");
            }

            this.stream = File.Open(path, FileMode.Open);

            this.Txd = (new Txd()).Read(this.stream);
            this.stream.Close();
        }

        public void Write(string path)
        {
            using (Stream stream = new MemoryStream())
            {
                this.Txd.Write(stream);

                byte[] buffer = new byte[stream.Length];
                stream.Position = 0;
                int bytesRead = stream.Read(buffer, 0, (int)stream.Length);

                File.WriteAllBytes(path, buffer);
            }
        }
    }
}
