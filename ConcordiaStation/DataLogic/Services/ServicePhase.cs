using ConcordiaStation.Data.Services.Interfaces;
using Newtonsoft.Json;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using Serilog;
using ConcordiaStation.Data.Enum;

namespace ConcordiaStation.Data.Services
{
    public class ServicePhase : IServicePhase
    {
        private readonly IRepositoryPhase _repositoryPhase;
        public ServicePhase(IRepositoryPhase repository) => _repositoryPhase = repository;

        public ServicePhase()
        {
        }

        public Phase UpdateStatus(Phase phase, Status newStatus) => _repositoryPhase.Update(phase! with { Status = newStatus });

        public string ShowDetails(Phase phase) => JsonConvert.SerializeObject(phase);

        public IEnumerable<Phase> GetAllPhases()
        {
            try
            {
                return _repositoryPhase.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Si è verificato un errore durante il recupero delle fasi");
                return Enumerable.Empty<Phase>();
            }
        }

        public Phase GetPhaseById(int phaseId) 
        {
            try
            {
                return _repositoryPhase.GetById(phaseId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Si è verificato un errore durante il recupero della Fase");
                return new Phase();
            }
        }
    }
}
