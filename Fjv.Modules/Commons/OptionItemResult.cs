using System;

namespace Fjv.Modules.Commons
{
    public class OptionItemResult
    {
        public string Name { get; set; }
        public bool SeparatedArguments { get; set; }
        public Type[] ArgumentsTypes { get; set; }
        public string Message { get; set; }
    }
}