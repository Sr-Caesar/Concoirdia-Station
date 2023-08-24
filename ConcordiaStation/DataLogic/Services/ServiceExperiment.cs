using ConcordiaStation.Data.Enum;
using ConcordiaStation.Data.Exceptions;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using ConcordiaStation.Data.Services.Interfaces;
using Serilog;

namespace ConcordiaStation.Data.Services
{
    public class ServiceExperiment : IServiceExperiment
    {
        private readonly IRepositoryExperiment _experimentRepository;
        private readonly IRepositoryPhase _phaseRepository;
        public ServiceExperiment(IRepositoryExperiment experimentRepository, IRepositoryPhase phaseRepository)
        {
            _experimentRepository = experimentRepository;
            _phaseRepository = phaseRepository;
        }
        public IEnumerable<Experiment> GetAllExperiments()
        {
            try
            {
                return _experimentRepository.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Si è verificato un errore durante il recupero degli esperimenti");
                return Enumerable.Empty<Experiment>();
            }
        }

        public Experiment AddPhase(int experimentId, Phase phase)
        {
            var myExperiment = _experimentRepository.GetById(experimentId) ?? throw new ExperimentNotFoundException();
            myExperiment.Phases.Add(phase);
            _experimentRepository.Update(myExperiment);
            return myExperiment;
        }

        public Experiment OrderPhaseByPriority(Experiment experiment)
        {
            experiment.Phases.Sort((phase1, phase2) => phase2.Priority.CompareTo(phase1.Priority));
            return experiment;
        }

        public Experiment UpdatePriorityOnDeadline(Experiment experiment)
        {
            var runningOutOfTimePhases = experiment.Phases.
                Where(phase => phase.Priority != Priority.HighPriority).
                Where(phase => phase.Status != Status.Finished).
                Where(phase => phase.Deadline < DateTime.Now.AddDays(5)).
                ToList();

            var updatedPhases = experiment.Phases.Select(phase =>
                runningOutOfTimePhases.Contains(phase)
                    ? phase with { Priority = Priority.RunningOutOfTime }
                    : phase
            ).ToList();

            var experimentUpdated = experiment with { Phases = updatedPhases };
            var result = _experimentRepository.Update(experimentUpdated);
            return result;
        }

        public List<string> ShowTitles(Experiment experiment)
        {
            var phases = experiment.Phases;
            var allTitles = new List<string>();
            foreach (Phase phase in phases)
            {
                allTitles.Add(phase.Title);
            }
            return allTitles;
        }
    }
}
