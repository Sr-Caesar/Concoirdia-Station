using ConcordiaStation.Data.Models;
namespace ConcordiaStation.WebApp.ViewModels
{
    public class CardIndexViewModel
    {
        public CardIndexViewModel(List<Experiment> experiment, List<Phase> phases)
        {
            Experiment = experiment;
            Phases = phases;
        }
        public CardIndexViewModel() : this(new List<Experiment>(), new List<Phase>())
        {
        }
        public List<Experiment> Experiment { get;}
        public List<Phase> Phases { get;}
        public string Title { get; set; } = string.Empty;
    }
}
