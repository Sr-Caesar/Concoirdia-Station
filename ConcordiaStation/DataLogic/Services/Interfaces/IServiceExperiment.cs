using ConcordiaStation.Data.Models;
namespace ConcordiaStation.Data.Services.Interfaces
{
    public interface IServiceExperiment
    {
        public Experiment AddPhase(int experimentId, Phase phase);
        public Experiment OrderPhaseByPriority(Experiment experiment);
        public Experiment UpdatePriorityOnDeadline(Experiment experiment);
        public IEnumerable<Experiment> GetAllExperiments();
        public List<string> ShowTitles(Experiment experiment);
    }
}
