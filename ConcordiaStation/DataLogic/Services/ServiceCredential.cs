using ConcordiaStation.Data.Repositories;

namespace ConcordiaStation.Data.Services
{
    public class ServiceCredential : IServiceCredential
    {
        private readonly IRepositoryCredential _credentialRepository;
        public ServiceCredential(IRepositoryCredential repository) => _credentialRepository = repository;

        public int? GetIdByEmail(string email)
        {
            var allIDs = _credentialRepository.GetAll();
            var userFoundByEmail = allIDs.SingleOrDefault(x => x.Email == email);
            return userFoundByEmail.Id;
        }

        public bool CheckEmail(string eMail)
        {
            var allEmails = _credentialRepository.GetAll();
            return allEmails.Any(x => x.Email == eMail);
        }

        public bool CheckPassword(string password)
        {
            var allPasswords = _credentialRepository.GetAll();
            return allPasswords.Any(x => VerifyPassword(password, x.Password));
        }

        private static string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        private static bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
