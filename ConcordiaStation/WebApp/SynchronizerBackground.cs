using System;
using ConcordiaStation.SyncApp;

namespace ConcordiaStation.WebApp
{
	public class SynchronizerBackground : BackgroundService
	{
		
		private readonly IConfiguration _configuration;
		private readonly string _urlBoardTrello;
		private readonly int _minutesIntervalSyncronizer;
		protected bool IsSynchronizing { get; private set; }
        protected bool SynchronizationCompleted { get; private set; }

        public SynchronizerBackground(IConfiguration configuration)
		{
			_configuration = configuration;
			_minutesIntervalSyncronizer = _configuration.GetValue<int>("SynchronizationBackGroundService:minutes");
			_urlBoardTrello = _configuration.GetValue<string>("TrelloConnection:urlBoard");
		}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
			var synchronizer = new SetUpSynchronizer();
			while (!stoppingToken.IsCancellationRequested)
			{
				SynchronizationCompleted = await IsBoardTrelloAvailableAsync();
				if (!SynchronizationCompleted)
				{
					continue;
				}
				IsSynchronizing = true;
				await synchronizer.RunAsync();
				IsSynchronizing = false;
                await Task.Delay(TimeSpan.FromMinutes(_minutesIntervalSyncronizer), stoppingToken);
            }
        }

		private async Task<bool> IsBoardTrelloAvailableAsync()
		{
			try
			{
				using var httpClient = new HttpClient();
				var request = new HttpRequestMessage(HttpMethod.Head, _urlBoardTrello);
				var response = await httpClient.SendAsync(request);
				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
    }
}

