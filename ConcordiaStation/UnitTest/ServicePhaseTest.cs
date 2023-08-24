using ConcordiaStation.Data.Enum;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using ConcordiaStation.Data.Services;
using Moq;

namespace ConcordiaStation.Test
{
    public class ServicePhaseTest
    {
        private readonly Mock<IRepositoryPhase> _mockRepositoryPhase;
        private readonly ServicePhase _sut;
        private readonly List<Phase> _phases;
        public ServicePhaseTest()
        {
            _phases = new List<Phase>
            {
                new Phase
                {
                    Id = 1,
                    Title = "Phase 1",
                    Description = "Phase description",
                    Deadline = DateTime.Now.AddDays(7),
                    Priority = Priority.HighPriority,
                    Status = Status.InExecution,
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            Content = "Comment 1",
                            PublicationDate = DateTime.Now
                        }
                    }
                },
                new Phase
                {
                    Id = 2,
                    Title = "Phase 2"
                }
            };

            _mockRepositoryPhase = new Mock<IRepositoryPhase>();
            _sut = new ServicePhase(_mockRepositoryPhase.Object);
        }

        [Fact]
        public void Test_ShouldUpdateStatus()
        {
            var newStatus = Status.Finished;

            _mockRepositoryPhase.Setup(r => r.Update(It.Is<Phase>(p => p.Id == _phases[0].Id)))
                .Returns((Phase phase) =>
                {
                    var updatedPhase = phase with { Status = newStatus };
                    return updatedPhase;
                });

            var result = _sut.UpdateStatus(_phases[0], newStatus);

            Assert.Equal(Status.Finished, result.Status);
        }

        [Fact]
        public void Test_ShouldNotThrowErrorWhileShowingDetails()
        {
            var exception = Record.Exception(() => _sut.ShowDetails(_phases[0]));

            Assert.Null(exception);
        }

        [Fact]
        public void Test_ShouldGetAllPhases()
        {
            _mockRepositoryPhase.Setup(r => r.GetAll())
                .Returns(_phases);

            var result = _sut.GetAllPhases();

            Assert.NotNull(result);
            Assert.Equal(_phases.AsEnumerable(), result);
        }

        [Fact]
        public void Test_ShouldGetPhaseById()
        {
            var phaseId = 1;

            _mockRepositoryPhase.Setup(r => r.GetById(phaseId))
                .Returns(_phases[0]);

            var result = _sut.GetPhaseById(phaseId);

            Assert.NotNull(result);
            Assert.Equal(_phases[0], result);
        }
    }
}

