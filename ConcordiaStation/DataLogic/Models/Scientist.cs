namespace ConcordiaStation.Data.Models
{
    public record Scientist (int? Id = default, string IdTrello = "", string GivenName = "", string FamilyName = "") : Entity (Id)
    {
        public int? PhaseId { get; set; } 
        public Phase Phase { get; set; } = default;
    }
}
