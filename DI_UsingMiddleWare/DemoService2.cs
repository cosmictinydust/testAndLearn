using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_UsingMiddleWare
{
    public class DemoService2 : IDemoService
    {
        public string Version => "v2";

        public void Run()
        {
            Console.WriteLine("The second class implemented service");
        }
    }
}
