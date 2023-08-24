using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories;
using ConcordiaStation.Data.Services;
using Moq;

namespace ConcordiaStation.Test
{
    public class ServiceCredentialTest
    {
        private readonly Mock<IRepositoryCredential> _mockRepository;
        private readonly ServiceCredential _sut;

        public ServiceCredentialTest()
        {
            _mockRepository = new Mock<IRepositoryCredential>();
            _sut = new ServiceCredential(_mockRepository.Object);
        }

        [Fact]
        public void Test_ShouldGetIdByEmail()
        {
            var email = "test@example.com";
            var expectedId = 123;
            var credentials = new List<Credential>
            {
                new Credential { Email = "other@example.com", Id = 456 },
                new Credential { Email = "test@example.com", Id = 123 },
                new Credential { Email = "another@example.com", Id = 789 }
            };

            _mockRepository.Setup(r => r.GetAll()).Returns(credentials);

            var result = _sut.GetIdByEmail(email);

            Assert.Equal(expectedId, result);
        }

        [Fact]
        public void Test_ShouldCheckEmail()
        {
            var email = "example@example.it";
            var credentials = new List<Credential>
            {
                new Credential { Email = "example@example.it" },
                new Credential { Email = "other@example.com" },
                new Credential { Email = "another@example.com" }
            };

            _mockRepository.Setup(r => r.GetAll()).Returns(credentials);

            var result = _sut.CheckEmail(email);

            Assert.True(result);
        }

        [Fact]
        public void Test_ShouldCheckPassword()
        {
            var password = "NotAPassword";
            var credentials = new List<Credential>
            {
                new Credential { Password = "$2a$11$0rUBBvIXiErmSrYBg0V7WOWUa50QJgW9ja7Ob9v0QUpzYmSGdOlz6" },
                new Credential { Password = "$2a$11$S7TmIVYmRxDQTMZbmUyWp.bRqU0WjtgFxj.xylhg6U3KdkjN3pIlq" },
                new Credential { Password = "$2a$11$1mj.Jwn/HzygM/IhhzKnjula23RbjLsZtDzZAJ2FZJH03.JffiG1q" }
            };

            _mockRepository.Setup(r => r.GetAll()).Returns(credentials);

            var result = _sut.CheckPassword(password);

            Assert.True(result);
        }
    }
}
