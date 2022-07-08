using System.Text.RegularExpressions;
using System.Xml.Linq;
using Fjv.Modules;
using Fjv.Modules.Attributes;
using Fjv.Modules.Commons;

namespace Samples.Shell.Modules
{
    [Module("show", ModuleRunningControl.Unique)]
    public class ShowModule : IDefaultModule
    {
        string _content;
        byte[] _input;

        string _pattern = @"{{attribute}}=([""'])(?:(?=(\\?))\2.)*?\1";

        public byte[] Load(byte[] input, string[] args, int index)
        {
            _content = System.Text.Encoding.UTF8.GetString(input);

            _input = input;

            return input;
        }

        [Option("links")]
        public byte[] GetLinks()
        {
            RunRegex(_content, "href");

            return _input;
        }

        [Option("sources")]
        public byte[] GetImages()
        {
            RunRegex(_content, "src");

            return _input;
        }

        private void RunRegex(string input, string attribute)
        {
            var regex = new Regex(_pattern.Replace("{{attribute}}", attribute), RegexOptions.IgnoreCase);

            foreach(Match match in regex.Matches(_content))
            {
                Console.WriteLine(match.Value);
            }
        }
    }
}