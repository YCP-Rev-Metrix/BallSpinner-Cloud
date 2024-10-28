using Client;
using System.Net;

internal class Program
{
    private static readonly FeaturedAPI Connection = new("https://localhost:7238/api/");

    private static readonly APIRequestErrorHandler LoginOn401 = new APIRequestErrorHandler().AddCondition(HttpStatusCode.Unauthorized, async () => await Connection.Login("testies", "J0rd@n"));

    private static async Task Main(string[] args)
    {
        _ = await Connection.Login("testies", "J0rd@n");

        Thread.Sleep(1000);

        _ = await Connection.Logout();

        Thread.Sleep(1000);
    }

}