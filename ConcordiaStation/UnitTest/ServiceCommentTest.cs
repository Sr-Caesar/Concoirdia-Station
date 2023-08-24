using ConcordiaStation.Data.Dto;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using ConcordiaStation.Data.Services;
using Moq;

namespace ConcordiaStation.Test
{
    public class ServiceCommentTest
    {
        private readonly Mock<IRepositoryComment> _mockRepositoryComment;
        private readonly ServiceComment _sut;
        private readonly List<Comment> _comments;
        public ServiceCommentTest()
        {
            _comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Content = "Comment 1",
                    PublicationDate = DateTime.Now,
                    ScientistId = 1,
                    PhaseId = 1
                },
                new Comment
                {
                    Id = 2,
                    Content = "Comment 2",
                    PublicationDate = DateTime.Now,
                    ScientistId = 2,
                    PhaseId = 1
                },
                new Comment
                {
                    Id = 3,
                    Content = "Comment 3",
                    PublicationDate = DateTime.Now,
                    ScientistId = 3,
                    PhaseId = 1
                }
            };

            _mockRepositoryComment = new Mock<IRepositoryComment>();
            _sut = new ServiceComment(_mockRepositoryComment.Object);
        }

        [Fact]
        public void Test_ShouldUpdateLastActivity()
        {
            var result = _sut.GetLastActivity(_comments[0]);

            Assert.Equal(_comments[0].PublicationDate, result);
        }

        [Fact]
        public void Test_ShouldGetCommentsByPhaseId()
        {
            var phaseId = 1;
            _mockRepositoryComment.Setup(r => r.GetByPhaseId(phaseId))
                .Returns(_comments.AsEnumerable());

            var result = _sut.GetCommentsByPhaseId(phaseId);

            Assert.NotNull(result);
            Assert.Equal(_comments.AsEnumerable(), result);
        }

        [Fact]
        public void Test_ShouldInsertComment()
        {
            _mockRepositoryComment.Setup(r => r.Insert(_comments[0]))
                .Returns(_comments[0]);

            var result = _sut.InsertComment(_comments[0]);

            Assert.NotNull(result);
            Assert.Equal(_comments[0], result);
        }
    }
}
