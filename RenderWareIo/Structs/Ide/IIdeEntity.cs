using System;
using System.Collections.Generic;
using System.Text;

namespace RenderWareIo.Structs.Ide
{
    public interface IIdeEntity<T>
    {
        T Read(string line);
        string Write();
    }
}
