using Fjv.Modules.Generic;
using Fjv.Modules.Attributes;
using Fjv.Modules.Commons;

namespace Samples.Shell.Modules 
{
    [Module("connect", ModuleRunningControl.Unique)]
    public class ConnectionModule : IArgumentableModuleAsync<string, string, string>
    {
        string _url = string.Empty;

        public EventHandler OnConnect;
        public EventHandler OnConnected;
        public EventHandler OnError;

        public ConnectionModule()
        {
            OnConnect += RaiseOnConnect;
            OnConnected += RaiseOnConnected;
            OnError += RaiseOnError;
        }

        public async Task<string> LoadAsync(string input, string moduleArgument, string[] args, int index, CancellationToken cancellationToken = default)
        {
            _url = moduleArgument;

            OnConnect?.Invoke(this, EventArgs.Empty);

            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(_url, cancellationToken);

                if(result.IsSuccessStatusCode)
                {
                    try
                    {
                        var values = await result.Content.ReadAsStringAsync(cancellationToken);

                        OnConnected?.Invoke(this, EventArgs.Empty);

                        return values;
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(this, EventArgs.Empty);

                        Console.WriteLine(ex.Message);
                    }
                }

                return string.Empty;
            }
        }

        private void RaiseOnConnect(object? sender, EventArgs e)
        {
            Console.Write("Connecting...");
        }

        private void RaiseOnConnected(object? sender, EventArgs e)
        {
            Console.WriteLine("ok!");
        }

        private void RaiseOnError(object? sender, EventArgs e)
        {
            throw new Exception("Something went wrong.");
        }
    }
}