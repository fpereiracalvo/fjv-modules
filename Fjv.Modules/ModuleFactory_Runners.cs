using System;
using System.Threading.Tasks;
using Fjv.Modules.Commons;
using Fjv.Modules.Extensions;

namespace Fjv.Modules
{
    public partial class ModuleFactory : ModuleFactoryBase, IModuleFactory
    {
        public virtual byte[] Run(string[] args, byte[] buffer = null)
        {
            var modules = this.GetModulesItems(args);

            foreach (var item in modules)
            {
                try
                {
                    if(item.Module.HasRunnningControl(ModuleRunningControl.ControlTaker))
                    {
                        return Run(item, this, buffer);
                    }

                    buffer = Run(item, this, buffer);    
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, new ModuleExceptionEventArgument(item, ex));

                    throw;
                }
            }

            return buffer;

        }

        public virtual byte[] Run(ModuleItem module, ModuleFactory moduleFactory, byte[] input)
        {
            byte[] result;

            this.OnModuleExecuting?.Invoke(this, new ModuleEventArgument(module));

            if (module.Module.IsArgumentableModule())
            {
                result = (module.Module as IArgumentableModule).Load(input, module.ModuleArgument, module.GlobalArguments, module.IndexArgument);
            }
            else
            {
                result = (module.Module as IDefaultModule).Load(input ?? module.ModuleArgument, module.GlobalArguments, module.IndexArgument);
            }

            this.OnModuleExecuted?.Invoke(this, new ModuleEventArgument(module));

            foreach (var option in module.Options)
            {
                this.OnOptionExecuting?.Invoke(this,  new OptionEventArgument(module, option));

                result = (byte[])moduleFactory.Invoke(module.Module, option.Name, option.Arguments);

                this.OnOptionExecuted?.Invoke(this,  new OptionEventArgument(module, option));
            }

            //todo: dispose module.

            return result;
        }

        public virtual async Task<byte[]> RunAsync(string[] args, byte[] buffer = null)
        {
            var modules = this.GetModulesItems(args);

            foreach (var item in modules)
            {
                try
                {
                    if(item.Module.HasRunnningControl(ModuleRunningControl.ControlTaker))
                    {
                        return await RunAsync(item, this, buffer);
                    }

                    buffer = await RunAsync(item, this, buffer);    
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, new ModuleExceptionEventArgument(item, ex));

                    throw;
                }
            }

            return buffer;
        }

        public virtual async Task<byte[]> RunAsync(ModuleItem module, ModuleFactory moduleFactory, byte[] input)
        {
            byte[] result;

            this.OnModuleExecuting?.Invoke(this, new ModuleEventArgument(module));

            if(module.Module.IsArgumentableModule())
            {
                result = await (module.Module as IArgumentableModuleAsync).LoadAsync(input, module.ModuleArgument, module.GlobalArguments, module.IndexArgument);
            }
            else
            {
                result = await (module.Module as IDefaultModuleAsync).LoadAsync(input ?? module.ModuleArgument, module.GlobalArguments, module.IndexArgument);
            }

            this.OnModuleExecuted?.Invoke(this, new ModuleEventArgument(module));

            foreach (var option in module.Options)
            {
                this.OnOptionExecuting?.Invoke(this,  new OptionEventArgument(module, option));

                result = await (Task<byte[]>)moduleFactory.Invoke(module.Module, option.Name, option.Arguments);

                this.OnOptionExecuted?.Invoke(this, new OptionEventArgument(module, option));
            }

            //todo: dispose module.

            return result;
        }
    }
}