using System.Text;
using ConcordiaStation.Data.Services.Interfaces;
using ConcordiaStation.Report.Interfaces;

namespace ConcordiaStation.Report
{
    public class DataProvider : IDataProvider
    {
        private readonly IServiceScientist _rScientist;
        private readonly IServiceExperiment _rExperiment;
        private readonly IServicePhase _rPhase;

        public DataProvider(IServiceScientist rScientist, IServiceExperiment rExperiment, IServicePhase rPhase)
        {
            _rScientist = rScientist;
            _rExperiment = rExperiment;
            _rPhase = rPhase;
        }

        public string GenerateReportText()
        {
            var taskReport = GenerateTaskReport();
            var scientistReport = GenerateScientistReport();
            return $"Hi everyone,\r\n\r\nThis is an automated message to inform you about the task distribution for the upcoming experiments." +
                $"{taskReport}\r\n{scientistReport}";
        }

        private string GenerateTaskReport()
        {
            var allExperiments = _rExperiment.GetAllExperiments();
            var allPhases = _rPhase.GetAllPhases();

            var taskReport = new StringBuilder();
            taskReport.AppendLine("Here is an updated breakdown of tasks statuses:").
                AppendLine($"We have a total of {allPhases.Count()} Phases, which are divided into {allExperiments.Count()} Experiments.").
                AppendLine($" - Tasks Completed: {allPhases.Count(phase => phase.Status == Data.Enum.Status.Finished)}").
                AppendLine($" - Tasks In Execution: {allPhases.Count(phase => phase.Status == Data.Enum.Status.InExecution)}").
                AppendLine($" - Tasks Not Started: {allPhases.Count(phase => phase.Status == Data.Enum.Status.NotImplemented)}\r\n").
                AppendLine("Each experiment consists of a specific number of tasks as outlined below:\r\n");
            taskReport = allExperiments.Aggregate(taskReport, (current, experiment) =>
                    current.AppendLine($"{experiment.Title}:").AppendLine($" - Total Tasks: {experiment.Phases.Count}\r\n"));

            return taskReport.ToString();
        }

        private string GenerateScientistReport()
        {
            var allScientists = _rScientist.GetAllScientists();

            var scientistReport = new StringBuilder();
            scientistReport.AppendLine($"Currently, we have a total of {allScientists.Count()} Scientists working at Concordia Station.");
            scientistReport.AppendLine("Here is an updated breakdown of scientists analytics:\r\n");
            scientistReport = allScientists.Aggregate(scientistReport, (current, scientist) =>
                current.AppendLine($"{scientist.GivenName} {scientist.FamilyName}:")
                .AppendLine($"Currently, he/she's working at {scientist.Phase.Title}\r\n"));

            return scientistReport.ToString();
        }
    }
}