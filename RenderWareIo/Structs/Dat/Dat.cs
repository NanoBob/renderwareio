using System;
using System.Collections.Generic;
using System.Text;

namespace RenderWareIo.Structs.Dat
{
    public struct Dat
    {
        public List<string> Ipls;
        public List<string> Ides;

        private void ParseLine(string line)
        {
            if (line.TrimStart().StartsWith("#"))
            {
                return;
            }

            string[] splits = line.Split(' ');

            switch (splits[0])
            {
                case "IPL":
                    this.Ipls.Add(splits[1]);
                    break;
                case "IDE":
                    this.Ides.Add(splits[1]);
                    break;
            }
        }

        public Dat Read(string content)
        {
            this.Ipls = new List<string>();
            this.Ides = new List<string>();

            string[] lines = content.Split('\n');
            foreach (string line in lines)
            {
                this.ParseLine(line);
            }

            return this;
        }

        public string Write()
        {
            string value = "objs";

            return value;
        }
    }
}
