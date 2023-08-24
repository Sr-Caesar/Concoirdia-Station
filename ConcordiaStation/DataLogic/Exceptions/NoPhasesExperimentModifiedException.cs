using System;
namespace ConcordiaStation.Data.Exceptions
{
	public class NoPhasesExperimentModifiedException : Exception
	{
		public NoPhasesExperimentModifiedException(string message) : base(message)
		{
		}
	}
}

