using System;
using System.Collections.Generic;
using System.Text;

namespace RenderWareIo.Structs.Dat
{
    public interface DatEntity<T>
    {
        T Read(string line);
        string Write();
    }
}
