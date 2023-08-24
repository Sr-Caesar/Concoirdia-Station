namespace ConcordiaStation.WebApp.SecurityServices.Interfaces
{
    public interface IServiceToken
    {
        public string GenerateToken(string email, int? id, int expirationMinutes = 480);
        public bool ValidateToken(string token);
        public void AddExpiredToken(string token, string filePath);
    }
}