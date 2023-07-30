using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fjv.Modules;
using Fjv.Modules.Attributes;

namespace HostWebClient.Modules
{
    [Module("-p")]
    public class PrepareWebPageModule : IDefaultModule
    {
        byte[] _url = new byte[]{};

        public byte[] Load(byte[] input, string[] args, int index)
        {
            return new byte[]{};
        }

        [Option("--a")]
        public byte[] SetUrl(string url)
        {
            _url = Encoding.UTF8.GetBytes(url);

            return _url;
        }
    }
}