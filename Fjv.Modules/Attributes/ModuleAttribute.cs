namespace Fjv.Modules.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ModuleAttribute : System.Attribute
    {
        readonly string _modulename;
        public string ModuleName => _modulename;
        
        public ModuleAttribute(string modulename)
        {
            _modulename = modulename;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class OptionAttribute : System.Attribute
    {
        readonly string optionname;

        public string OptionName => optionname;

        public OptionAttribute(string optionname)
        {
            this.optionname = optionname;
        }
    }
}