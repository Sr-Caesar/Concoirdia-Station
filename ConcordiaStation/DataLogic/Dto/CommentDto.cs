namespace ConcordiaStation.Data.Dto
{
	public class CommentDto
	{
		public int Id { get; set; }
		public PhaseDto Phase { get; set; }
		public ScientistDto Scientist { get; set; }
		public string IdPhaseTrello { get; set; }
		public string IdCommentTrello { get; set; }
		public string Content { get; set; }
		public DateTime PublicationDate { get; set; }
	}
}

