using System.Collections.Generic;

namespace RenderWareIo.Structs.Ide
{
    public struct Ide
    {
        private string lastHeader;

        public List<Obj> Objs;
        public List<Tobj> Tobjs;
        public List<Anim> Anims;
        public List<Txdp> Txdps;
        public List<Ped> Peds;
        public List<Weapon> Weapons;

        private void ParseLine(string line)
        {
            if (line.TrimStart().StartsWith("#"))
                return;

            if (!line.Contains(",") && line.Trim().Trim('\r').Length > 0)
            {
                lastHeader = line.Trim('\r').Trim();
                return;
            }

            if (line.Trim('\r').Trim().Length == 0)
                return;

            switch (lastHeader)
            {
                case "objs":
                    Objs.Add(new Obj().Read(line));
                    break;
                case "tobj":
                    Tobjs.Add(new Tobj().Read(line));
                    break;
                case "anim":
                    Anims.Add(new Anim().Read(line));
                    break;
                case "txdp":
                    Txdps.Add(new Txdp().Read(line));
                    break;
                case "peds":
                    Peds.Add(new Ped().Read(line));
                    break;
                case "weap":
                    Weapons.Add(new Weapon().Read(line));
                    break;
            }
        }

        public Ide Read(string content)
        {
            this.Objs = new List<Obj>();
            this.Tobjs = new List<Tobj>();
            this.Anims = new List<Anim>();
            this.Txdps = new List<Txdp>();
            this.Peds = new List<Ped>();
            this.Weapons = new List<Weapon>();

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
