using ConcordiaStation.Data.Enum;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using ConcordiaStation.Data.Services;
using Moq;

namespace ConcordiaStation.Test
{
    public class ServiceExperimentTest
    {
        private readonly Mock<IRepositoryExperiment> _mockRepositoryExperiment;
        private readonly Mock<IRepositoryPhase> _mockRepositoryPhase;
        private readonly ServiceExperiment _sut;
        private readonly List<Experiment> _experiments;

        public ServiceExperimentTest()
        {
            _experiments = new List<Experiment>()
            {
                new Experiment
                {
                    Id = 1,
                    Title = "Sample Experiment",
                    Phases = new List<Phase>
                    {
                        new Phase
                        {
                            Id = 1,
                            Title = "Phase 1",
                            Description = "Phase description",
                            Deadline = DateTime.Now.AddDays(7),
                            Priority = Priority.LowPriority,
                            Status = Status.InExecution,
                        },
                        new Phase
                        {
                            Id = 2,
                            Title = "Phase 2",
                            Deadline = DateTime.Now.AddDays(2),
                            Priority = Priority.LowPriority,
                            Status = Status.NotImplemented
                        },
                        new Phase
                        {
                            Id = 3,
                            Title = "Phase 3",
                            Deadline = DateTime.Now.AddDays(4),
                            Priority = Priority.HighPriority,
                            Status = Status.Finished
                        },
                        new Phase
                        {
                            Id = 4,
                            Title = "Phase 4",
                            Deadline = DateTime.Now.AddDays(5),
                            Priority = Priority.MediumPriority,
                            Status = Status.NotImplemented
                        },
                        new Phase
                        {
                            Id = 5,
                            Title = "Phase 5",
                            Deadline = DateTime.Now.AddDays(1),
                            Priority = Priority.LowPriority,
                            Status = Status.Finished
                        },
                        new Phase
                        {
                            Id = 6,
                            Title = "Phase 6",
                            Deadline = DateTime.Now.AddHours(119),
                            Priority = Priority.MediumPriority,
                            Status = Status.NotImplemented
                        }
                    }
                }
            };

            _mockRepositoryExperiment = new Mock<IRepositoryExperiment>();
            _mockRepositoryPhase = new Mock<IRepositoryPhase>();
            _sut = new ServiceExperiment(_mockRepositoryExperiment.Object, _mockRepositoryPhase.Object);   
        }

        [Fact]
        public void Test_ShouldGetAllExperiments()
        {
            _mockRepositoryExperiment.Setup(r => r.GetAll())
                .Returns(_experiments);

            var result = _sut.GetAllExperiments();

            Assert.NotNull(result);
            Assert.Equal(_experiments.AsEnumerable(), result);
        }

        [Fact]
        public void Test_ShouldAddPhase()
        {
            var experimentId = 1;
            var phaseToAdd = new Phase
            {
                Id = 1,
                Title = "Phase 1",
                Description = "Phase description",
                Deadline = DateTime.Now.AddDays(7),
                Priority = Priority.LowPriority,
                Status = Status.InExecution
            };
            var rightExperiment = new Experiment
            {
                Id = 1,
                Title = "Sample Experiment",
                Phases = new List<Phase> { _experiments[0].Phases[0] }
            };

            _mockRepositoryExperiment.Setup(r => r.GetById(experimentId)).Returns(rightExperiment);
            rightExperiment.Phases.Add(phaseToAdd);
            _mockRepositoryExperiment.Setup(r => r.Update(rightExperiment))
                .Returns(rightExperiment);

            var result = _sut.AddPhase(experimentId, _experiments[0].Phases[1]);

            Assert.Equal(rightExperiment.Id, result.Id);
            Assert.Equal(rightExperiment.Title, result.Title);
            Assert.Equal(rightExperiment.Phases[0].Id, result.Phases[0].Id);
            Assert.Equal(rightExperiment.Phases[0].Title, result.Phases[0].Title);
            Assert.Equal(rightExperiment.Phases[0].Description, result.Phases[0].Description);
            Assert.Equal(rightExperiment.Phases[0].Deadline, result.Phases[0].Deadline);
            Assert.Equal(rightExperiment.Phases[0].Priority, result.Phases[0].Priority);
            Assert.Equal(rightExperiment.Phases[0].Status, result.Phases[0].Status);
        }

        [Fact]
        public void Test_ShouldOrderExperimentByPriority()
        {
            var result = _sut.OrderPhaseByPriority(_experiments[0]);

            Assert.Equal(Priority.HighPriority, result.Phases[0].Priority);
            Assert.Equal(Priority.MediumPriority, result.Phases[1].Priority);
            Assert.Equal(Priority.MediumPriority, result.Phases[2].Priority);
            Assert.Equal(Priority.LowPriority, result.Phases[3].Priority);
            Assert.Equal(Priority.LowPriority, result.Phases[4].Priority);
            Assert.Equal(Priority.LowPriority, result.Phases[5].Priority);
        }

        [Fact]
        public void Test_ShouldUpdatePriorityOnDeadline()
        {
            var runningOutOfTimePhases = _experiments[0].Phases.
                Where(phase => phase.Priority != Priority.HighPriority).
                Where(phase => phase.Status != Status.Finished).
                Where(phase => phase.Deadline < DateTime.Now.AddDays(5)).
                ToList();
            var updatedPhases = _experiments[0].Phases.Select(phase =>
                runningOutOfTimePhases.Contains(phase)
                    ? phase with { Priority = Priority.RunningOutOfTime }
                    : phase
            ).ToList();

            _mockRepositoryExperiment.Setup(r => r.Update(It.IsAny<Experiment>()))
                .Returns((Experiment updatedExperiment) =>
                {
                    updatedExperiment.Phases = updatedPhases;
                    return updatedExperiment;
                });

            var result = _sut.UpdatePriorityOnDeadline(_experiments[0]);

            Assert.Equal(Priority.LowPriority, result.Phases[0].Priority);
            Assert.Equal(Priority.RunningOutOfTime, result.Phases[1].Priority);
            Assert.Equal(Priority.HighPriority, result.Phases[2].Priority);
            Assert.Equal(Priority.RunningOutOfTime, result.Phases[3].Priority);
            Assert.Equal(Priority.LowPriority, result.Phases[4].Priority);
            Assert.Equal(Priority.RunningOutOfTime, result.Phases[5].Priority);
        }

        [Fact]
        public void Test_ShouldShowTitles()
        {
            var result = _sut.ShowTitles(_experiments[0]);

            Assert.Contains("Phase 1", result);
            Assert.Contains("Phase 2", result);
            Assert.Contains("Phase 3", result);
        }
    }
}