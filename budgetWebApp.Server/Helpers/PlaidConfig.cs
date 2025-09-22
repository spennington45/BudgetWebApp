namespace budgetWebApp.Server.Helpers;
public static class PlaidConfig
{
    public static string ClientId =>
        System.Environment.GetEnvironmentVariable("PLAID_CLIENT_ID") ?? throw new InvalidOperationException("PLAID_CLIENT_ID is not set.");

    public static string SandboxSecret =>
        System.Environment.GetEnvironmentVariable("PLAID_SANDBOX_SECRET") ?? throw new InvalidOperationException("PLAID_SANDBOX_SECRET is not set.");

    public static string Environment =>
        System.Environment.GetEnvironmentVariable("PLAID_ENV") ?? "sandbox"; 
}