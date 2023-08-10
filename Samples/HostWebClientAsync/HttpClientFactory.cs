using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HostWebClientAsync
{
    public class HttpClientFactory
    {
        IConfiguration _configuration;
        
        public HttpClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HttpClient GetHttpClient()
        {
            return new HttpClient();
        }
    }
}