using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_UsingMiddleWare
{
    public class DemoService1 : IDemoService
    {
        public string Version => "v1";

        public void Run()
        {
            Console.WriteLine("The first class implemented service");
        }
    }
}
