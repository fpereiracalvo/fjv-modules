using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fjv.Modules;
using Fjv.Modules.Attributes;

namespace HostWebClientAsync.Modules
{
    [Module("-r")]
    public class ReaderWebPageModule : IDefaultModuleAsync
    {
        byte[] _bytes = new byte[]{};
        string _content = string.Empty;

         public async Task<byte[]> LoadAsync(byte[] input, string[] args, int index)
        {
            return await Task.Run(()=>{
                _bytes = input;
                _content = Encoding.UTF8.GetString(input);

                return input;
            });
        }

        [Option("--all")]
        public async Task<byte[]> ShowSource()
        {
            return await Task.Run(()=>{
                Console.WriteLine(_content);

                return _bytes;
            });
        }

        [Option("--ec")]
        public async Task<byte[]> GetElementContent(string element)
        {
            var pattern = $"<{element}>(.*?)</{element}>";

            var matches = Regex.Matches(_content, pattern);

            return await Task.Run(()=>{
                if(!matches.Any())
                {
                    pattern = $"<{element}\\s+([^>]+)>(.*?)</{element}>";
                    matches = Regex.Matches(_content, pattern);
                }

                foreach (Match match in matches)
                {
                    string matchedText = match.Groups[1].Value;
                    
                    Console.WriteLine(matchedText);
                }

                return _bytes;
            });
        }
    }
}