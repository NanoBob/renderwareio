using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RenderWareIo.Structs.Ipl
{
    public struct Ipl
    {
        private string lastHeader;

        public List<Inst> Insts;

        private void ParseLine(string line)
        {
            if (line.TrimStart().StartsWith("#"))
            {
                return;
            }

            if (!line.Contains(","))
            {
                lastHeader = line.Trim();
                return;
            }

            switch (lastHeader)
            {
                case "inst":
                    Insts.Add(new Inst().Read(line));
                    break;
            }
        }

        public Ipl Read(string content)
        {
            this.Insts = new List<Inst>();

            string[] lines = content.Split('\n');
            foreach(string line in lines)
            {
                this.ParseLine(line);
            }

            return this;
        }

        public string Write()
        {
            string value = "objs";
            value += "\n" + this.Insts.Select(i => i.Write());

            return value;
        }
    }
}
