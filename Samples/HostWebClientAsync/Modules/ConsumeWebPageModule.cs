using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fjv.Modules;
using Fjv.Modules.Attributes;

namespace HostWebClientAsync.Modules
{
    [Module("-c")]
    [ModuleHelp("Consume a web page and save the content for the next module.")]
    public class ConsumeWebPageModule : IDefaultModuleAsync
    {
        HttpClient _httpClient;

        public ConsumeWebPageModule(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> LoadAsync(byte[] input, string[] args, int index)
        {
            try
            {
                var url = Encoding.UTF8.GetString(input);
                var result = await _httpClient.GetByteArrayAsync(url);

                return result;
            }
            catch (Exception ex)
            {
                //end console with exit 1
                Console.WriteLine(ex.Message);
                Environment.Exit(1);

                return null;
            }
        }
    }
}