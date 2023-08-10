using System.Collections.Generic;

namespace Fjv.Modules.Commons
{
    public class ModuleItem
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public IModule Module { get; set; }
        public byte[] ModuleArgument { get; set; }
        public string[] GlobalArguments { get; set; }
        public int IndexArgument { get; set; }
        
        public List<OptionItem> Options { get; set; }

        public ModuleItem()
        {
            Options = new List<OptionItem>();
        }
    }

    public class OptionItem
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public object[] Arguments { get; set; }
    }
}