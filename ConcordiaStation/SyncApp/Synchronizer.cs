using System;
using ConcordiaStation.Data.Dto;
using ConcordiaStation.SyncApp.Endpoints.Gateways;
using Microsoft.Extensions.Configuration;

namespace ConcordiaStation.SyncApp
{
    public class Synchronizer : ISynchronizer
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly ITrelloGateway _trelloGateway;
        private const string BOARD_ID = "647859b1cc887944369a104c";

        public Synchronizer(IDatabaseGateway databaseGateway, ITrelloGateway trelloGateway)
        {
            _databaseGateway = databaseGateway;
            _trelloGateway = trelloGateway;
        }

        public async Task SynchronizeDataAsync()
        {

            var contentFromTrello = await _trelloGateway.GetExperimentDtoFromTrelloBoard(BOARD_ID);
            var contentFromLocalDb = _databaseGateway.GetExperimentDtoFromLocalDb();
            var contentNotDuplicated = CheckDifferenceDto(contentFromTrello, contentFromLocalDb);
            _databaseGateway.UpdateDatabaseWithTrelloContent(contentNotDuplicated.database);
            var experimentsWithIdTrello = await _trelloGateway.UpdateTrelloBoardWithLocalDbContentAsync(contentNotDuplicated.trello, BOARD_ID);
            _databaseGateway.UpdateExperimentsWithIdTrello(experimentsWithIdTrello);
        }

        private (List<ExperimentDto> trello, List<ExperimentDto> database) CheckDifferenceDto(List<ExperimentDto> experimentsFromTrello, List<ExperimentDto> experimentsFromDb)
        {
            var listDtoForTrello = new List<ExperimentDto>();
            var listDtoForLocalDb = new List<ExperimentDto>();

            foreach (var experimentTrello in experimentsFromTrello)
            {
                var experimentDb = experimentsFromDb.SingleOrDefault(x => x.Id == experimentTrello.Id || x.IdListTrello == experimentTrello.IdListTrello);

                if (experimentDb != null)
                {
                    foreach (var phase in experimentTrello.PhasesDto)
                    {
                        var phaseDb = experimentDb.PhasesDto.SingleOrDefault(x => x.Id == phase.Id || x.IdPhaseInTrello == phase.IdPhaseInTrello);
                        if (phaseDb != null && phaseDb.LastActivity > phase.LastActivity)
                        {
                            if (!listDtoForTrello.Contains(experimentDb))
                            {
                                listDtoForTrello.Add(experimentDb);
                            }
                        }
                        else
                        {
                            if (!listDtoForLocalDb.Contains(experimentTrello))
                            {
                                listDtoForLocalDb.Add(experimentTrello);
                            }
                        }
                    }
                }
                else
                {
                    listDtoForLocalDb.Add(experimentTrello);
                }
            }
            return (listDtoForTrello, listDtoForLocalDb);
        }
    }
}