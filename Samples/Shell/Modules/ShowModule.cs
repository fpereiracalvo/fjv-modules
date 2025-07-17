using System.Text.RegularExpressions;
using Fjv.Modules.Generic;
using Fjv.Modules.Attributes;
using Fjv.Modules.Commons;

namespace Samples.Shell.Modules
{
    [Module("show", ModuleRunningControl.Unique)]
    public class ShowModule : IDefaultModuleAsync<string, string>
    {
        string _content = string.Empty;

        string _pattern = @"{{attribute}}=([""'])(?:(?=(\\?))\2.)*?\1";

        public async Task<string> LoadAsync(string input, string[] args, int index, CancellationToken cancellationToken = default)
        {
            _content = input;

            return await Task.FromResult(input);
        }

        [Option("links")]
        public async Task<string> GetLinksAsync()
        {
            RunRegex(_content, "href");

            return await Task.FromResult(_content);
        }

        [Option("sources")]
        public async Task<string> GetImagesAsync()
        {
            RunRegex(_content, "src");

            return await Task.FromResult(_content);
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