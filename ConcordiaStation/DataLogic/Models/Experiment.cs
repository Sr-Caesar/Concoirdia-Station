namespace ConcordiaStation.Data.Models
{
    public record Experiment (int? Id = default,string IdListTrello = "", string Title = "") : Entity (Id)
    {
        public List<Phase> Phases { get; set; } = new List<Phase>();
    }
}
