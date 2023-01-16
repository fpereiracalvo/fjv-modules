using System;

namespace Fjv.Modules.Commons
{
    internal class OptionItemResult
    {
        public string Name { get; set; }
        public bool SeparatedArguments { get; set; }
        public Type[] ArgumentsTypes { get; set; }
    }
}