namespace Travel_psw.Services
{
    public class AwardBackgroundService : BackgroundService
    {
        private readonly AwardService _awardService;

        public AwardBackgroundService(AwardService awardService)
        {
            _awardService = awardService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
                await _awardService.AwardAuthorsAsync();
                await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
            }
        }
    }

}
