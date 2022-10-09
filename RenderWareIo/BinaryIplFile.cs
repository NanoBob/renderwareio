using RenderWareIo.Structs.BinaryIpl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo
{
    public class BinaryIplFile
    {
        Stream stream;

        public BinaryIpl BinaryIpl { get; set; }


        public BinaryIplFile()
        {
            this.stream = new MemoryStream();

            this.BinaryIpl = new BinaryIpl();
        }

        public BinaryIplFile(byte[] data)
        {
            this.stream = new MemoryStream(data);

            this.BinaryIpl = (new BinaryIpl()).Read(this.stream);
            this.stream.Close();
        }

        public BinaryIplFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"BinaryIpl file '{path}' does not exist");
            }

            this.stream = File.Open(path, FileMode.Open);

            this.BinaryIpl = BinaryIpl.Read(this.stream);
        }

        public void Write(string path)
        {
            using (Stream stream = new MemoryStream())
            {
                this.BinaryIpl.Write(stream);

                byte[] buffer = new byte[stream.Length];
                stream.Position = 0;
                int bytesRead = stream.Read(buffer, 0, (int)stream.Length);

                File.WriteAllBytes(path, buffer);
            }

        }
    }
}
