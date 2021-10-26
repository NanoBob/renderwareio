using RenderWareIo.Structs.Col;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo
{
    public class ColFile
    {
        Stream stream;

        public Col Col { get; set; }


        public ColFile()
        {
            this.stream = new MemoryStream();

            this.Col = new Col();
        }

        public ColFile(byte[] data)
        {
            this.stream = new MemoryStream(data);

            this.Col = Col.Read(this.stream);
            this.stream.Close();
        }

        public ColFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Col file '{path}' does not exist");
            }

            this.stream = File.Open(path, FileMode.Open);

            this.Col = (new Col()).Read(this.stream);
            this.stream.Close();
        }

        public void Write(string path)
        {
            using (Stream stream = new MemoryStream())
            {
                this.Col.Write(stream);

                byte[] buffer = new byte[stream.Length];
                stream.Position = 0;
                int bytesRead = stream.Read(buffer, 0, (int)stream.Length);

                File.WriteAllBytes(path, buffer);
            }

        }
    }
}
