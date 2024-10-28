namespace Server;

/// <summary>
/// Provides configuration from appsettings.json files
/// </summary>
public static class Config
{
    private static readonly IConfiguration Configuration;

    public static readonly string AuthAudience;
    public static readonly string AuthIssuer;
    public static readonly int AuthSecretLength;

    static Config()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .Build();

        AuthAudience = Configuration["ApiSettings:Auth:Audience"] ?? "RevMetrix";
        AuthIssuer = Configuration["ApiSettings:Auth:Issuer"] ?? "https://localhost:7238/";
        AuthSecretLength = Configuration.GetValue<int>("ApiSettings:Auth:SecretLength", 32);
    }
}
