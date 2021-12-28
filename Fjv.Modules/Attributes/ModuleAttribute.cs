using System;
using Fjv.Modules.Commons;

namespace Fjv.Modules.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ModuleAttribute : System.Attribute
    {
        readonly string _moduleName;
        readonly ModuleRunningControl _runningControl;
        public string ModuleName => _moduleName;
        public ModuleRunningControl RunningControl => _runningControl;
        
        public ModuleAttribute(string modulename)
        {
            _moduleName = modulename;
            _runningControl = ModuleRunningControl.NotSet;
        }
        
        public ModuleAttribute(string moduleName, ModuleRunningControl runningControl)
            : this(moduleName)
        {
            if((runningControl & ModuleRunningControl.Input) == ModuleRunningControl.Input &&
            (runningControl & ModuleRunningControl.Output) == ModuleRunningControl.Output)
            {
                throw new Exception($"The module {moduleName} cannot being an input and output at the same time.");
            }
            _runningControl = runningControl;
        }
    }
}