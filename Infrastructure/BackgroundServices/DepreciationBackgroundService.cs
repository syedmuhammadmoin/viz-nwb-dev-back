using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundServices
{
    public class DepreciationBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _services;

        public DepreciationBackgroundService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Set the time for the first run at 3PM
            var now = DateTime.Now;
            var scheduledTime = new DateTime(now.Year, now.Month, now.Day, 14, 56, 0);

            if (now > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            var interval = scheduledTime - now;

            // Set the timer to run the task at 3PM every day
            _timer = new Timer(DoWork, null, interval, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var unitOfWork = new UnitOfWork(dbContext);

                // Perform the user update logic here
                // ...

                await unitOfWork.SaveAsync();
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
