using ConcordiaStation.Report.Interfaces;

namespace ConcordiaStation.Report
{
    public class ReportManager : IReportManager
    {
        private readonly ISender _sender;
        private readonly IDataProvider _dataProvider;

        public ReportManager(ISender EmailSender, IDataProvider DataProvider)
        {
            _sender = EmailSender;
            _dataProvider = DataProvider;
        }

        public void CreateAndSendReport()
        {
            try
            {
                var report = _dataProvider.GenerateReportText();
                var pdf = DataExporter.GeneratePdf(report);
                _sender.SendEmail(pdf);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}

