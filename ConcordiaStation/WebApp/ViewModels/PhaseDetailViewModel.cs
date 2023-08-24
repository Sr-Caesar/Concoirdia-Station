using ConcordiaStation.Data.Enum;
using ConcordiaStation.Data.Models;

namespace ConcordiaStation.WebApp.ViewModels
{
    public class PhaseDetailViewModel
    {
        public PhaseDetailViewModel(IEnumerable<Scientist> scientists, Experiment experiment, IEnumerable<Comment> comment)
        {
            Scientists = scientists;
            Experiment = experiment;
            Comments = comment;
        }
        public PhaseDetailViewModel() : this(new List<Scientist>(), new Experiment(), new List<Comment>())
        {
        }
        public int Id { get; set; } 
        public string Title { get; set; } 
        public string Description { get; set; } 
        public DateTime Deadline { get; set; } 
        public Priority Priority { get; set; } 
        public Status Status { get; set; } 
        public string LastComment { get; set; } 
        public IEnumerable<Scientist> Scientists { get; set; } 
        public Experiment Experiment { get; set; } 
        public IEnumerable<Comment> Comments { get; set;} 
        public string NewComment { get; set; } 
    }
}
