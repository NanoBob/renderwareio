using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace RenderWareBuilders
{
    public class Material
    {
        internal ushort Index { get; set; }
        public string Name { get; set; } = "";
        public string MaskName { get; set; } = "";
        public Color Color { get; set; } = Color.White;
    }
}
