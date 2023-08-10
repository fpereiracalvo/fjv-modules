using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostWebClientAsync
{
    public class HostArguments
    {
        public string[] GetArguments()
        {
            return Environment.GetCommandLineArgs().Skip(1).ToArray();
        }
    }
}