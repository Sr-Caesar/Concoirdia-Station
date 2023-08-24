using ConcordiaStation.Data.Dto;
using ConcordiaStation.Data.Enum;
using ConcordiaStation.Data.Models;

namespace ConcordiaStation.Data.Services.Interfaces
{
    public interface IServiceScientist
    {
        public IEnumerable<Scientist> GetAllScientists();
        public Scientist AssignPhase(int scientistId, PhaseDto phaseDto);
        public Phase AddComment(int phaseId, string comment);
        public Phase ChangeStatusPhase(int phaseId, Status newStatus);
    }
}
