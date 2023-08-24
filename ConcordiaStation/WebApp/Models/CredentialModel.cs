namespace ConcordiaStation.WebApp.Models
{
    public class CredentialModel
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ShowPassword { get; set; }
    }
}
