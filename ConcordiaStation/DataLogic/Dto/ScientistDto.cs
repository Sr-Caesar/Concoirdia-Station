
namespace ConcordiaStation.Data.Dto
{
    public class ScientistDto
    {
        public int Id { get; set; }
        public string IdTrello { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public List<PhaseDto> Phases { get; set; }
    }
}
