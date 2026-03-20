namespace budgetWebApp.Server.Models.DTOs
{
    public class PlaidLinkRequestDto
    {
        public long UserId { get; set; }
        public string PublicToken { get; set; } = null!;
        public string? InstitutionName { get; set; }
        public List<PlaidAccountDto> Accounts { get; set; } = new();

    }
}
