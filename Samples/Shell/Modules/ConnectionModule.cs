using Fjv.Modules;
using Fjv.Modules.Attributes;
using Fjv.Modules.Commons;

namespace Samples.Shell.Modules 
{
    [Module("connect", ModuleRunningControl.Unique)]
    public class ConnectionModule : IArgumentableModule
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

        public byte[] Load(byte[] input, byte[] moduleArgument, string[] args, int index)
        {
            _url = System.Text.Encoding.UTF8.GetString(moduleArgument);

            OnConnect?.Invoke(this, EventArgs.Empty);

            using (var client = new HttpClient())
            {
                var task = Task.Run(async ()=> await client.GetAsync(_url));

                task.Wait();

                var result = task.Result;

                if(result.IsSuccessStatusCode)
                {
                    var contentTask = Task.Run<byte[]>(async ()=>{
                        try
                        {
                            var bytes = await result.Content.ReadAsByteArrayAsync();

                            OnConnected?.Invoke(this, EventArgs.Empty);

                            return bytes;
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                            return new byte[]{};
                        }
                    });
                    
                    contentTask.Wait();

                    return contentTask.Result;
                }

                OnError?.Invoke(this, EventArgs.Empty);

                return moduleArgument;
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