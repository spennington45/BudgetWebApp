namespace budgetWebApp.Server.Models.DTOs
{
    public class UserDto
    {
        public long UserId { get; set; }

        public string ExternalId { get; set; } = null!;

        public string? Name { get; set; }

        public string? Picture { get; set; }

        public string Email { get; set; } = null!;
    }
}
