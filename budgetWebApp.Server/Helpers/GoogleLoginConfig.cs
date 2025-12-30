namespace budgetWebApp.Server.Helpers
{
    public static class GoogleLoginConfig
    {
        public static string GoogleClientId =>
        System.Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? throw new InvalidOperationException("GOOGLE_CLIENT_ID is not set.");

        public static string GoogleClientSecret =>
            System.Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? throw new InvalidOperationException("GOOGLE_CLIENT_SECRET is not set.");
    }
}
