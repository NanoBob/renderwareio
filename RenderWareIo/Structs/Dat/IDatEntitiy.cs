using System;
using System.Collections.Generic;
using System.Text;

namespace RenderWareIo.Structs.Dat
{
    public interface IDatEntitiy<T>
    {
        T Read(string line);
        string Write();
    }
}
