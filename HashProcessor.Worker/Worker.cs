using HashProcessor.Worker.Services;

namespace HashProcessor.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IProcessHashesService _processHashesService;

        public Worker(ILogger<Worker> logger,
            IProcessHashesService processHashesService)
        {
            _logger = logger;
            _processHashesService = processHashesService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await _processHashesService.ProcessHashesAsync();
            }
        }
    }
}
