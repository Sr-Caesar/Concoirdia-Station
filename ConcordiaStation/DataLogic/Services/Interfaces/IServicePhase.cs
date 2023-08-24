using ConcordiaStation.Data.Enum;
using ConcordiaStation.Data.Models;
namespace ConcordiaStation.Data.Services.Interfaces
{
    public interface IServicePhase
    {
        public string ShowDetails(Phase phase);
        public IEnumerable<Phase> GetAllPhases();
        public Phase GetPhaseById(int phaseId);
        public Phase UpdateStatus(Phase phase, Status newStatus);
    }
}
