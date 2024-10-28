using Common.POCOs;
using System.Net.Http.Headers;

namespace Client;

/// <summary>
/// <see cref="APIConnection"/> with extra features such as automatic error handling and login
/// </summary>
public class FeaturedAPI : APIConnection
{
    /// <summary>
    /// Refresh token used to request new JWT
    /// </summary>
    public byte[]? RefreshToken = null;

    public FeaturedAPI(string baseUrl) : base(baseUrl) { }

    /// <summary>
    /// Sets default header authorization JWT
    /// </summary>
    /// <param name="token">JWT to set header to</param>
    public void SetAuthJWT(string token) => Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    /// <summary>
    /// Attempts to refresh JWT using <see cref="RefreshToken"/>. If unsucessful or not present, login using provided arguments
    /// </summary>
    /// <param name="username">User's username</param>
    /// <param name="password">User's password</param>
    /// <returns></returns>
    public async Task<bool> Login(string username, string password)
    {
        if (RefreshToken == null) // Refresh token does not exist
        {
            Console.WriteLine("Attempting to log in using username & password");
            APIResponse<DualToken> token = await Post<DualToken, UserIdentification>("User/Authorize", new("firstname", "lastname", username, password, "testing@gmail.com", "570-000-0000"));
            if (token.IsValid) // Logged in with user credentials
            {
                SetAuthJWT(token.Result!.TokenA);
                RefreshToken = token.Result!.TokenB;
                Console.WriteLine("Logged in successfully");
                return true;
            }
            else if (token.StatusCode == System.Net.HttpStatusCode.Unauthorized)  // Incorrect credentials
            {
                Console.WriteLine("Failed to log in: incorrect credentials");
                return false;
            }
            else if (token.StatusCode == System.Net.HttpStatusCode.Forbidden) // Incorrect privileges
            {
                Console.WriteLine("Failed to log in: incorrect privileges");
                return false;
            }
            else  // Unknown
            {
                Console.WriteLine("Failed to log in: unkown cause");
                return false;
            }
        }
        else // Refresh token exists
        {
            Console.WriteLine("Attempting to refresh authorization token");
            APIResponse<DualToken> refresh = await Post<DualToken, ByteArrayToken>("User/Refresh", new ByteArrayToken(RefreshToken));
            if (refresh.IsValid) // Refreshed token
            {
                SetAuthJWT(refresh.Result!.TokenA);
                RefreshToken = refresh.Result!.TokenB;
                Console.WriteLine("Refreshed authorization token successfully");
                return true;
            }
            else if (refresh.StatusCode == System.Net.HttpStatusCode.Unauthorized) // Old refresh token
            {
                RefreshToken = null;
                Console.WriteLine("Refresh token no longer valid");
                return await Login(username, password);
            }
            else if (refresh.StatusCode == System.Net.HttpStatusCode.Forbidden) // Incorrect privileges or user not in system
            {
                RefreshToken = null;
                Console.WriteLine("Failed to refresh authorization token: incorrect privileges");
                return await Login(username, password);
            }
            else // Unknown
            {
                Console.WriteLine("Failed to refresh authorization token: unkown cause");
                return false;
            }
        }
    }

    /// <summary>
    /// Attempts to log user out of their logged-in account
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Logout()
    {
        APIResponse result = await Delete("User/Logout");
        if (result.WasSuccessful)
        {
            RefreshToken = null;
            SetAuthJWT("");
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Execute <paramref name="apiRequest"/>. If applicable, retrieve matching action from <paramref name="errorHandler"/> using <see cref="APIResponse"/> as predicate. Execute action, then, re-execute <paramref name="apiRequest"/>.
    /// </summary>
    /// <param name="apiRequest">API request method (<see cref="APIConnection"/> or <see cref="FeaturedAPI"/>) to execute - lambda expressions</param>
    /// <param name="errorHandler">The error handler to execute using predicate (<paramref name="apiRequest"/> result)</param>
    /// <returns><see cref="APIResponse"/> representing result of first or second call to <paramref name="apiRequest"/></returns>
    public static async Task<APIResponse> ExecuteWithAutomaticErrorHandler(Func<Task<APIResponse>> apiRequest, APIRequestErrorHandler errorHandler)
    {
        APIResponse result = await apiRequest.Invoke();

        Func<Task>? action = errorHandler.Find(result);

        if (action == null) return result;

        await action.Invoke();

        return await apiRequest.Invoke();
    }

    /// <summary>
    /// Execute <paramref name="apiRequest"/>. If applicable, retrieve matching action from <paramref name="errorHandler"/> using <see cref="APIResponse{TResult}"/> as predicate. Execute action, then, re-execute <paramref name="apiRequest"/>.
    /// </summary>
    /// <typeparam name="TResult">POCO or object recieved from API request</typeparam>
    /// <param name="apiRequest">API request method (<see cref="APIConnection"/> or <see cref="FeaturedAPI"/>) to execute - lambda expressions</param>
    /// <param name="errorHandler">The error handler to execute using predicate (<paramref name="apiRequest"/> result)</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing result of first or second call to <paramref name="apiRequest"/></returns>
    public static async Task<APIResponse<TResult>> ExecuteWithAutomaticErrorHandler<TResult>(Func<Task<APIResponse<TResult>>> apiRequest, APIRequestErrorHandler errorHandler) where TResult : POCO
    {
        APIResponse<TResult> result = await apiRequest.Invoke();

        Func<Task>? action = errorHandler.Find(result);

        if (action == null) return result;

        await action.Invoke();

        return await apiRequest.Invoke();
    }
}
