using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fjv.Modules;
using Fjv.Modules.Attributes;

namespace HostWebClientAsync.Modules
{
    [Module("-p")]
    [ModuleHelp("Prepare a web page to be consumed by the next module.")]
    public class PrepareWebPageModule : IDefaultModuleAsync
    {
        byte[] _url = new byte[]{};

        public async Task<byte[]> LoadAsync(byte[] input, string[] args, int index)
        {
            return await Task.Run(()=>{
                return new byte[]{};
            });
        }

        [Option("--a")]
        [OptionHelp("Set the url to be used.")]
        public async Task<byte[]> SetUrl(string url)
        {
            return await Task.Run(()=>{
                _url = Encoding.UTF8.GetBytes(url);

                return _url;
            });
        }
    }
}