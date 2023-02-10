using System;
using System.Globalization;
using System.Linq;
using RenderWareIo.Structs.Ide;

namespace RenderWareIo.Structs.Dat
{
    public struct MeleeWeapon : IDatEntitiy<MeleeWeapon>
    {
        public string WeaponType { get; set; }
        public string FireType { get; set; }
        public float TargetRange { get; set; }
        public float WeaponRange { get; set; }
        public int Model1Id { get; set; }
        public int Model2Id { get; set; }
        public int WeaponSlot { get; set; }

        public string BaseCombo { get; set; }
        public int NumCombos { get; set; }
        public int Flags { get; set; }
        public string StealthAnimationGroup { get; set; }

        public MeleeWeapon Read(string line)
        {
            string[] splits = line.Split(' ').Select((split) => split.Trim()).ToArray();

            WeaponType = splits[1];
            FireType = splits[2];
            TargetRange = float.Parse(splits[3], CultureInfo.InvariantCulture);
            WeaponRange = float.Parse(splits[4], CultureInfo.InvariantCulture);
            Model1Id = int.Parse(splits[5]);
            Model2Id = int.Parse(splits[6]);
            WeaponSlot = int.Parse(splits[7]);

            BaseCombo = splits[8];
            NumCombos = int.Parse(splits[9]);
            Flags = int.Parse(splits[10], NumberStyles.HexNumber);
            StealthAnimationGroup = splits[11];

            return this;
        }

        public string Write()
        {
            throw new NotImplementedException();
        }
    }
}
