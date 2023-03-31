using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace RenderWareBuilders
{
    public class Vertex
    {
        internal ushort Index { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 Uv { get; set; }
    }

    public class PrelitVertex : Vertex
    {
        public Color Day { get; set; }
        public Color Night { get; set; }
    }
}
