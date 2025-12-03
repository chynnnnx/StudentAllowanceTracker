namespace StudentAllowanceTracker.Client.DTOs
{
    public class AuthTokenStore
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiry { get; set; }
    }

}
