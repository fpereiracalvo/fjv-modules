using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fjv.Modules;
using Fjv.Modules.Attributes;

namespace HostWebClient.Modules
{
    [Module("-r")]
    public class ReaderWebPageModule : IDefaultModule
    {
        byte[] _bytes = new byte[]{};
        string _content = string.Empty;

        public byte[] Load(byte[] input, string[] args, int index)
        {
            _bytes = input;
            _content = Encoding.UTF8.GetString(input);

            return input;
        }

        [Option("--all")]
        public byte[] ShowSource()
        {
            Console.WriteLine(_content);

            return _bytes;
        }

        [Option("--ec")]
        public byte[] GetElementContent(string element)
        {
            var pattern = $"<{element}>(.*?)</{element}>";

            var matches = Regex.Matches(_content, pattern);

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
        }
    }
}