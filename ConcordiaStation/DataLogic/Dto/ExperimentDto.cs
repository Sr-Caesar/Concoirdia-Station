using System;
namespace ConcordiaStation.Data.Dto
{
	public class ExperimentDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string IdListTrello { get; set; }
        public List<PhaseDto> PhasesDto { get; set; }
	}
}