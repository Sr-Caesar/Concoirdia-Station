using System;
using ConcordiaStation.Data.Dto;

namespace ConcordiaStation.SyncApp.Endpoints.Gateways
{
	public interface IDatabaseGateway
	{
		public List<ExperimentDto> GetExperimentDtoFromLocalDb();
		public void UpdateDatabaseWithTrelloContent(List<ExperimentDto> experimentDtoFromTrello);
		public void UpdateExperimentsWithIdTrello(List<ExperimentDto> experimentDtoWithIdTrello);
	}
}

