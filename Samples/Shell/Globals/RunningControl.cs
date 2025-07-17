namespace Samples.Shell.Globals
{
    public class RunningControl
    {
        static CancellationTokenSource _cancellationTokenSource = null!;
        public static CancellationTokenSource CancellationToken
        {
            get {
                _cancellationTokenSource ??= new CancellationTokenSource();

                return _cancellationTokenSource;
            }
        }
    }
}