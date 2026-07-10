namespace budgetWebApp.Server.Models.DTOs
{
    public class PlaidAccountDto
    {
        public string AccountId { get; set; } = null!;
        public string? Name { get; set; }
        public string? Mask { get; set; }
        public string? Type { get; set; }
        public string? Subtype { get; set; }
        public string? OfficialName { get; set; }
        public string? CurrentBalance { get; set; }
        public string? AvailableBalance { get; set; }
    }
}
