using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fjv.Modules;
using Fjv.Modules.Attributes;

namespace HostWebClient.Modules
{
    [Module("-prep-web")]
    public class ConsumeWebPageModule : IDefaultModule
    {
        HttpClient _httpClient;
        string _url = string.Empty;

        public ConsumeWebPageModule(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public byte[] Load(byte[] input, string[] args, int index)
        {
            return new byte[]{};
        }

        [Option("--address")]
        public byte[] SetUrl(string address)
        {
            _httpClient.BaseAddress = new Uri(address);

            return new byte[]{};
        }
    }
}