namespace ConcordiaStation.Data.Services
{
    public interface IServiceCredential
    {
        public int? GetIdByEmail(string email);
        public bool CheckEmail(string eMail);
        public bool CheckPassword(string password);
    }
}