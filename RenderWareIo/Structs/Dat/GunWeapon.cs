using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using RenderWareIo.Structs.Ide;

namespace RenderWareIo.Structs.Dat
{
    public struct GunWeapon : IDatEntitiy<GunWeapon>
    {
        public string WeaponType { get; set; }
        public string FireType { get; set; }
        public float TargetRange { get; set; }
        public float WeaponRange { get; set; }
        public int Model1Id { get; set; }
        public int Model2Id { get; set; }
        public int WeaponSlot { get; set; }

        public string AnimationGroupId { get; set; }
        public int AmmoinClip { get; set; }
        public int Damage { get; set; }
        public Vector3 FireOffset { get; set; }
        public int SkillLevel { get; set; }
        public int RequiredStat { get; set; }
        public float Accuracy { get; set; }
        public float MoveSpeed { get; set; }

        public int AnimLoopStart { get; set; }
        public int AnimLoopEnd { get; set; }
        public int AnimLoopFire { get; set; }

        public int AnimLoop2Start { get; set; }
        public int AnimLoop2End { get; set; }
        public int AnimLoop2Fire { get; set; }

        public int BreakoutTime { get; set; }

        public int Flags { get; set; }
        public float Speed { get; set; }
        public float Radius { get; set; }
        public float Lifespan { get; set; }
        public float Spread { get; set; }


        public GunWeapon Read(string line)
        {
            string[] splits = line.Split(' ').Select((split) => split.Trim()).ToArray();

            WeaponType = splits[1];
            FireType = splits[2];
            TargetRange = float.Parse(splits[3], CultureInfo.InvariantCulture);
            WeaponRange = float.Parse(splits[4], CultureInfo.InvariantCulture);
            Model1Id = int.Parse(splits[5]);
            Model2Id = int.Parse(splits[6]);
            WeaponSlot = int.Parse(splits[7]);


            AnimationGroupId = splits[8];
            AmmoinClip = int.Parse(splits[9]);
            Damage = int.Parse(splits[10]);
            FireOffset = new Vector3(
                float.Parse(splits[11], CultureInfo.InvariantCulture),
                float.Parse(splits[12], CultureInfo.InvariantCulture),
                float.Parse(splits[13], CultureInfo.InvariantCulture));
            SkillLevel = int.Parse(splits[14]);
            RequiredStat = int.Parse(splits[15]);
            Accuracy = float.Parse(splits[16], CultureInfo.InvariantCulture);
            MoveSpeed = float.Parse(splits[17], CultureInfo.InvariantCulture);

            AnimLoopStart = int.Parse(splits[18]);
            AnimLoopEnd = int.Parse(splits[19]);
            AnimLoopFire = int.Parse(splits[20]);

            AnimLoop2Start = int.Parse(splits[21]);
            AnimLoop2End = int.Parse(splits[22]);
            AnimLoop2Fire = int.Parse(splits[23]);

            BreakoutTime = int.Parse(splits[24]);

            Flags = int.Parse(splits[25], NumberStyles.HexNumber);

            if (splits.Length > 26)
            {
                Speed = float.Parse(splits[26], CultureInfo.InvariantCulture);
                Radius = float.Parse(splits[27], CultureInfo.InvariantCulture);
                Lifespan = float.Parse(splits[28], CultureInfo.InvariantCulture);
                Spread = float.Parse(splits[29], CultureInfo.InvariantCulture);
            }

            return this;
        }

        public string Write()
        {
            throw new NotImplementedException();
        }
    }
}
