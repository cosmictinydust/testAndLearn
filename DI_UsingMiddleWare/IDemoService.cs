using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_UsingMiddleWare
{
    public interface IDemoService
    {
        string Version { get; }
        void Run();
    }
}
