using System.Runtime.Serialization;

namespace ConcordiaStation.Data.Exceptions
{
    [Serializable]
    internal class ExperimentNotFoundException : Exception
    {
        public ExperimentNotFoundException()
        {
        }

        public ExperimentNotFoundException(string message) : base(message)
        {
        }

        public ExperimentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExperimentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}