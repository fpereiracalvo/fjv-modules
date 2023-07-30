using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fjv.Modules;
using Fjv.Modules.Attributes;

namespace HostWebClient.Modules
{
    [Module("-c")]
    public class ConsumeWebPageModule : IDefaultModule
    {
        HttpClient _httpClient;

        public ConsumeWebPageModule(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public byte[] Load(byte[] input, string[] args, int index)
        {
            var url = Encoding.UTF8.GetString(input);

            var task = _httpClient.GetByteArrayAsync(url);

            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                //end console with exit 1
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }

            return task.Result;
        }
    }
}