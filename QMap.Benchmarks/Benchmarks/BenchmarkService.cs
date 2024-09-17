namespace QMap.Benchmarks.Benchmarks
{
    public class BenchmarkService : IBenchmarkService
    {
        private Action _bechmarkRunner;

        private IEnumerable<Action> _conditions;

        public BenchmarkService(Action bechmarkRunner, IEnumerable<Action> conditions)
        {
            _bechmarkRunner = bechmarkRunner;

            _conditions = conditions;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_conditions.Count() > 0)
            {
                _conditions.ToList()
               .ForEach(a =>
               {
                   a?.Invoke();
               });
            }

            _bechmarkRunner.Invoke();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.Exit(0);

            return Task.CompletedTask;
        }
    }
}
