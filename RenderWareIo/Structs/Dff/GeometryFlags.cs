using System;
using System.Collections.Generic;
using System.Text;

namespace RenderWareIo.Structs.Dff
{
    [Flags]
    public enum GeometryFlags
    {
        TRISTRIP = 0x00000001,
        POSITIONS = 0x00000002,
        TEXTURED = 0x00000004,
        PRELIT = 0x00000008,
        NORMALS = 0x00000010,
        LIGHT = 0x00000020,
        MODULATE_MATERIAL_COLOR = 0x00000040,
        TEXTURED2 = 0x00000080,
        NATIVE = 0x01000000,
        NATIVE_INSTANCE = 0x02000000,
        FLAGS_MASK = 0x000000FF,
        NATIVE_FLAGS_MASK = 0x0F000000,
    }
}
