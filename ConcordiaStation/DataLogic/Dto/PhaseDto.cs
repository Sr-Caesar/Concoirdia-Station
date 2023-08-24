using ConcordiaStation.Data.Enum;
using ConcordiaStation.Data.Models;

namespace ConcordiaStation.Data.Dto
{
    public class PhaseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string IdPhaseInTrello { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime LastActivity { get; set; }
        public DateTime Deadline { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public List<ScientistDto> Scientists { get; set; }
        public ExperimentDto Eperiment { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
