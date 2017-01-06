using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsDoc
{
    using Microsoft.Extensions.CommandLineUtils;

    class Program
    {
        static void Main(string[] args)
        {
            var commandLineApplication =
                new CommandLineApplication(throwOnUnexpectedArg: false);            
        }
    }
}
