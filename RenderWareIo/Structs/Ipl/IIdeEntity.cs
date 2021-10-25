using System;
using System.Collections.Generic;
using System.Text;

namespace RenderWareIo.Structs.Ipl
{
    public interface IIplEntity<T>
    {
        T Read(string line);
        string Write();
    }
}
