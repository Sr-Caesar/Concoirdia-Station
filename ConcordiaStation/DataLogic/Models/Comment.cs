using System;
namespace ConcordiaStation.Data.Models
{
	public record Comment(int? Id = default, string Content = "", string IdPhaseTrello = "", string IdCommentTrello = "", DateTime PublicationDate = default): Entity(Id)
	{
        public int? PhaseId { get; set; }
        public Phase Phase { get; set; }
        public int? ScientistId { get; set; }
        public Scientist Scientist { get; set; }
    }
}

