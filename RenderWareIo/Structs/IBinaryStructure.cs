using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RenderWareIo.Structs
{
    public interface IBinaryStructure<T>
    {
        T Read(Stream stream);
        void Write(Stream stream);
        
    }
}
