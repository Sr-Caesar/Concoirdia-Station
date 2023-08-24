using ConcordiaStation.Data.Services.Interfaces;
using ConcordiaStation.Data.Dto;
using ConcordiaStation.Data.Enum;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using ConcordiaStation.Data.Exceptions;
using System.Threading.Tasks;
using Serilog;

namespace ConcordiaStation.Data.Services
{
    public class ServiceScientist : IServiceScientist
    {
        private readonly IRepositoryScientist _repositoryScientist;
        private readonly IRepositoryPhase _repositoriesPhase;

        public ServiceScientist(IRepositoryScientist repositoryScientist, IRepositoryPhase repositoriesPhase)
        {
            _repositoryScientist = repositoryScientist;
            _repositoriesPhase = repositoriesPhase;
        }

        public IEnumerable<Scientist> GetAllScientists()
        {
            try
            {
                return _repositoryScientist.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Si è verificato un errore durante il recupero degli scienziati");
                return Enumerable.Empty<Scientist>();
            }
        }

        public Scientist AssignPhase(int scientistId, PhaseDto phaseDto)
        {
            var existingScientist = _repositoryScientist.GetById(scientistId);
            if (existingScientist.Phase is not null) throw new ScientistNotFoundException();
            var phase = new Phase()
            {
                Id = phaseDto.Id,
                Title = phaseDto.Title,
                Description = phaseDto.Description,
                Deadline = phaseDto.Deadline,
                Priority = phaseDto.Priority,
                Status = phaseDto.Status
            };
            existingScientist.Phase = phase;
            _repositoryScientist.Update(existingScientist);
            return existingScientist;
        }

        public Phase AddComment(int phaseId, string comment)
        {
            var existingPhase = _repositoriesPhase.GetById(phaseId) ?? throw new PhaseNotFoundException();
            var commentToAdd = new Comment
            {
                Content = comment,
                PublicationDate = DateTime.Now,
                Phase = existingPhase,
            };
            existingPhase.Comments.Add(commentToAdd);
            _repositoriesPhase.Update(existingPhase);
            return existingPhase;
        }

        public Phase ChangeStatusPhase(int phaseId, Status newStatus)
        {
            var existingPhase = _repositoriesPhase.GetById(phaseId) ?? throw new PhaseNotFoundException();
            var updatedTask = existingPhase with { Status = newStatus };
            _repositoriesPhase.Update(updatedTask);
            return updatedTask;
        }
    }
}
