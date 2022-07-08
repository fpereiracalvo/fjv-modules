using Fjv.Modules.Commons;

namespace Fjv.Modules.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class OptionAttribute : System.Attribute
    {
        readonly string _optionName;
        public string OptionName => _optionName;

        public OptionAttribute(string optionname)
        {
            _optionName = optionname;
        }
    }
}