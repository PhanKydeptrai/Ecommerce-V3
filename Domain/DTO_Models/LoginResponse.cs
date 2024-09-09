namespace Domain.DTO_Models
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
