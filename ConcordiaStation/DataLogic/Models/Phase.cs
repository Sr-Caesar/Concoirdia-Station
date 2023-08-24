using ConcordiaStation.Data.Enum;

namespace ConcordiaStation.Data.Models
{
    public record Phase (int? Id = default, string IdCardTrello = "",string Title = "",DateTime DateOfCreation = default, DateTime LastActivity = default ,string Description = "", DateTime Deadline = default, Priority Priority = Priority.NotDefined, Status Status = Status.NotImplemented): Entity(Id)
    {
        public List<Scientist> Scientists  { get; set; } = new List<Scientist>();
        public Experiment Experiment { get; set; } = default;
        public List<Comment> Comments { get; set; } = new List<Comment>();
    } 
}
