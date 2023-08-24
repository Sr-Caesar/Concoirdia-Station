using System.Runtime.Serialization;

namespace ConcordiaStation.Data.Exceptions
{
    [Serializable]
    internal class ScientistNotFoundException : Exception
    {
        public ScientistNotFoundException()
        {
        }

        public ScientistNotFoundException(string message) : base(message)
        {
        }

        public ScientistNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ScientistNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}