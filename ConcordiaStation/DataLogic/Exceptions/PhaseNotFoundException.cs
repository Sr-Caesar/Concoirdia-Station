using System.Runtime.Serialization;

namespace ConcordiaStation.Data.Exceptions
{
    [Serializable]
    internal class PhaseNotFoundException : Exception
    {
        public PhaseNotFoundException()
        {
        }

        public PhaseNotFoundException(string message) : base(message)
        {
        }

        public PhaseNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PhaseNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}