namespace ConcordiaStation.Report.Interfaces
{
    public interface ISender
    {
        void SendEmail(params byte[][] attachments);
    }
}