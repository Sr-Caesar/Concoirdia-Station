using ConcordiaStation.Data.Enum;
using ConcordiaStation.Data.Services;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Dto;
using Moq;
using ConcordiaStation.Data.Repositories.Interfaces;

namespace UnitTesters
{
    public class ServiceScientistTest
    {
        private readonly Mock<IRepositoryScientist> _mockRepositoryScientist;
        private readonly Mock<IRepositoryPhase> _mockRepositoryPhase;
        private readonly ServiceScientist _sut;
        private readonly PhaseDto _dtoPhase;
        private readonly Phase _basePhase;
        private readonly Phase _existingPhase;
        private readonly List<Scientist> _scientists;

        public ServiceScientistTest()
        {
            _dtoPhase = new PhaseDto
            {
                Id = 1,
                Title = "Phase 1",
                Description = "Phase description",
                Deadline = DateTime.Parse("30-09-2025"),
                Priority = Priority.HighPriority,
                Status = Status.InExecution,
                Comments = new List<CommentDto>
                {
                    new CommentDto
                    {
                        Content = "COMMENTO MODIFICATO!",
                        PublicationDate = DateTime.Now
                    }
                }
            };

            _basePhase = new Phase
            {
                Id = 1,
                Title = "Phase 1",
                Description = "Phase description",
                Deadline = DateTime.Parse("30-09-2025"),
                Priority = Priority.HighPriority,
                Status = Status.InExecution,
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Content = "NEW COMMENT!",
                        PublicationDate = DateTime.Now
                    }
                }
            };

            _existingPhase = new Phase
            {
                Id = 1,
                Title = "Phase 1",
                Description = "Phase description",
                Deadline = DateTime.Parse("30-09-2025"),
                Priority = Priority.HighPriority,
                Status = Status.InExecution,
                Comments = new List<Comment>()
            };

            _scientists = new List<Scientist>
            {
                new Scientist {
                    Id = 1,
                    GivenName = "John",
                    FamilyName = "Doe"
                },
                new Scientist {
                    Id = 2,
                    GivenName = "Sam",
                    FamilyName = "Smith"
                },
                new Scientist {
                    Id = 3,
                    GivenName = "Gino",
                    FamilyName = "Cerrutti"
                }
            };

            _mockRepositoryScientist = new Mock<IRepositoryScientist>();
            _mockRepositoryPhase = new Mock<IRepositoryPhase>();
            _sut = new ServiceScientist(_mockRepositoryScientist.Object, _mockRepositoryPhase.Object);
        }

        [Fact]
        public void Test_ShouldGetAllScientists()
        {
            _mockRepositoryScientist.Setup(r => r.GetAll())
                .Returns(_scientists);
            
            var result = _sut.GetAllScientists();

            Assert.NotNull(result);
            Assert.Equal(_scientists.AsEnumerable(), result);
        }

        [Fact]
        public void Test_ShouldAssignPhase()
        {
            var scientistId = 1;

            _mockRepositoryScientist.Setup(r => r.GetById(scientistId))
                .Returns(_scientists[0]);
            _mockRepositoryScientist.Setup(r => r.Update(_scientists[0]))
                .Returns((Scientist updatedScientist) =>
                {
                    updatedScientist.Phase = _basePhase;
                    return updatedScientist;
                });

            var result = _sut.AssignPhase(1, _dtoPhase);

            Assert.Equal(_scientists[0].Id, result.Id);
            Assert.Equal(_scientists[0].GivenName, result.GivenName);
            Assert.Equal(_scientists[0].FamilyName, result.FamilyName);
            Assert.Equal(_dtoPhase.Id, result.Phase.Id);
            Assert.Equal(_dtoPhase.Title, result.Phase.Title);
            Assert.Equal(_dtoPhase.Description, result.Phase.Description);
            Assert.Equal(_dtoPhase.Deadline, result.Phase.Deadline);
            Assert.Equal(_dtoPhase.Priority, result.Phase.Priority);
            Assert.Equal(_dtoPhase.Status, result.Phase.Status);
        }

        [Fact]
        public void Test_ShouldAddComment()
        {
            var phaseId = 1;
            var comment = "NEW COMMENT!";

            _mockRepositoryPhase.Setup(r => r.GetById(phaseId))
                .Returns(_existingPhase);
            var commentToAdd = new Comment
            {
                Content = comment,
                PublicationDate = DateTime.Now,
                Phase = _existingPhase,
            };
            _existingPhase.Comments.Add(commentToAdd);
            _mockRepositoryPhase.Setup(r => r.Update(_existingPhase))
                .Returns(_existingPhase);

            var result = _sut.AddComment(phaseId, comment);

            Assert.Equal(_basePhase.Comments.Last().Content, result.Comments.Last().Content);
        }

        [Fact]
        public void Test_ShouldChangeStatus()
        {
            var phaseId = 1;
            var newStatus = Status.InExecution;

            _mockRepositoryPhase.Setup(r => r.GetById(phaseId))
                .Returns(_existingPhase);
            var updatedTask = _existingPhase with { Status = newStatus };
            _mockRepositoryPhase.Setup(r => r.Update(updatedTask))
                .Returns(updatedTask);

            var result = _sut.ChangeStatusPhase(phaseId, newStatus);

            Assert.Equal(newStatus, result.Status);
        }
    }
}