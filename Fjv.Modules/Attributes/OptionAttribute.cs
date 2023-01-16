using Fjv.Modules.Commons;

namespace Fjv.Modules.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class OptionAttribute : System.Attribute
    {
        readonly string _optionName;
        readonly bool _separatedArgument;
        public string OptionName => _optionName;
        public bool SeparatedArgument => _separatedArgument;

        public OptionAttribute(string optionname)
        {
            _optionName = optionname;
        }

        public OptionAttribute(string optionname, bool separatedArgument)
            : this(optionname)
        {
            _separatedArgument = separatedArgument;
        }
    }
}