using RenderWareIo.Structs.Dff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo
{
    public class DffFile
    {
        Stream stream;

        public Dff Dff { get; set; }


        public DffFile()
        {
            this.stream = new MemoryStream();

            this.Dff = new Dff();
        }

        public DffFile(Dff dff)
        {
            this.Dff = dff;
        }

        public DffFile(byte[] data)
        {
            this.stream = new MemoryStream(data);

            this.Dff = (new Dff()).Read(this.stream);
        }

        public DffFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Dff file '{path}' does not exist");
            }

            this.stream = File.Open(path, FileMode.Open);

            this.Dff = (new Dff()).Read(this.stream);
            this.stream.Close();
        }

        public void Write(string path)
        {
            using (Stream stream = new MemoryStream())
            {
                this.Dff.Write(stream);

                byte[] buffer = new byte[stream.Length];
                stream.Position = 0;
                int bytesRead = stream.Read(buffer, 0, (int)stream.Length);

                File.WriteAllBytes(path, buffer);
            }

        }
    }
}
