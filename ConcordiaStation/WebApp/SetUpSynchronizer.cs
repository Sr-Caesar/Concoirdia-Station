using System;
using ConcordiaStation.Data.Context;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories;
using ConcordiaStation.Data.Services;
using ConcordiaStation.Report;
using ConcordiaStation.SyncApp.Endpoints.Gateways;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConcordiaStation.SyncApp
{
	public class SetUpSynchronizer
	{
		private readonly IConfiguration _configuration;

		public SetUpSynchronizer()
		{
			var currentDir = Directory.GetCurrentDirectory();
			_configuration = new ConfigurationBuilder()
				.SetBasePath(currentDir)
				.AddJsonFile("appsettings.json")
				.Build();
		}

        public async Task RunAsync()
		{
			string boardId = _configuration["TrelloConnection:boardId"];
			string url = _configuration["TrelloConnection:urlBoard"];
            var options = new DbContextOptionsBuilder<ConcordiaLocalDbContext>()
               .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
               .Options;
            var context = new ConcordiaLocalDbContext(options);
            var repositoryExperiment = new RepositoryExperiment(context);
            var repositoryPhase = new RepositoryPhase(context);
            var repositoryScientist = new RepositoryScientist(context);
            var repositoryComment = new RepositoryComment(context);
            ITrelloGateway gatewayTrello = new TrelloGateway(_configuration);
			IDatabaseGateway databaseGateway = new DatabaseGateway(repositoryExperiment,repositoryPhase,repositoryScientist, repositoryComment);
			ISynchronizer synchronizer = new Synchronizer(databaseGateway,gatewayTrello);

			await synchronizer.SynchronizeDataAsync();

            //report 
            MailKit.Net.Smtp.SmtpClient smtpClient = new();
            var sExperiment = new ServiceExperiment(repositoryExperiment, repositoryPhase);
            var sScientist = new ServiceScientist(repositoryScientist, repositoryPhase);
            var sPhase = new ServicePhase(repositoryPhase);
            var sender = new Sender(smtpClient, _configuration);
            var dataProvider = new DataProvider(sScientist, sExperiment, sPhase);
            var report = new ReportManager(sender, dataProvider);
            report.CreateAndSendReport();
        }
	}
}

