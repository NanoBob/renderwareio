using RenderWareIo.Structs.Dff;
using RenderWareIo.Structs.Img;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo
{
    public class ImgFile
    {
        Stream stream;

        public Img Img { get; set; }


        public ImgFile()
        {
            this.stream = new MemoryStream();

            this.Img = new Img();
        }

        public ImgFile(byte[] data)
        {
            this.stream = new MemoryStream(data);

            this.Img = Img.Read(this.stream);
            this.stream.Close();
        }

        public ImgFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Txd file '{path}' does not exist");
            }

            this.stream = File.Open(path, FileMode.Open);

            this.Img = Img.Read(this.stream);
            this.stream.Close();
        }

        public void Write(string path)
        {
            using (Stream stream = new MemoryStream())
            {
                this.Img.Write(stream);

                byte[] buffer = new byte[stream.Length];
                stream.Position = 0;
                int bytesRead = stream.Read(buffer, 0, (int)stream.Length);

                File.WriteAllBytes(path, buffer);
            }

        }
    }
}
