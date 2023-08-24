using System;
using ConcordiaStation.Data.Dto;

namespace ConcordiaStation.SyncApp.Endpoints.Gateways
{
	public interface ITrelloGateway
	{
        public Task<List<ExperimentDto>> GetExperimentDtoFromTrelloBoard(string boardId);
        public Task<List<ExperimentDto>> UpdateTrelloBoardWithLocalDbContentAsync(List<ExperimentDto> experimentDtoFromLocalDb, string boardId);
    }
}

