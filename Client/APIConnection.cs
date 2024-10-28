using Common.POCOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Client;

/// <summary>
/// Provides the ability to connect and make requests to a remote or local web API using the following request types:
/// <list type="bullet">
/// <item>Get: Retrieve resource from the server</item>
/// <item>Post: Create new resource on the server</item>
/// <item>Put: Update existing resource on the server</item>
/// <item>Patch: Update part of existing resource on the server</item>
/// <item>Delete: Delete existing resource</item>
/// </list>
/// </summary>
public class APIConnection
{
    protected readonly string URL; // https://localhost:7238/api/
    protected readonly HttpClient Client = new();

    public APIConnection(string baseUrl)
    {
        URL = baseUrl;
        Client.BaseAddress = new Uri(URL);
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    #region Get
    /// /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Get<TResult>(string url) where TResult : POCO => await Get<TResult>(url, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Get(string url) => await Get(url, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Get<TResult>(string url, UrlParameters urlParameters) where TResult : POCO => await Get<TResult>(url + urlParameters.ToUrlParameters());

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Get(string url, UrlParameters urlParameters) => await Get(url + urlParameters.ToUrlParameters());

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Get<TResult>(string url, UrlParameters urlParameters, CancellationToken cancellationToken) where TResult : POCO => await Get<TResult>(url + urlParameters.ToUrlParameters(), cancellationToken);

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Get(string url, UrlParameters urlParameters, CancellationToken cancellationToken) => await Get(url + urlParameters.ToUrlParameters(), cancellationToken);

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Get<TResult>(string url, CancellationToken cancellationToken) where TResult : POCO
    {
        Console.WriteLine("Get Issued");

        HttpResponseMessage response = await Client.GetAsync(url, cancellationToken);

        return await GenerateResult<TResult>("Get", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Get(string url, CancellationToken cancellationToken)
    {
        Console.WriteLine("Get Issued");

        HttpResponseMessage response = await Client.GetAsync(url, cancellationToken);

        return GenerateResult("Get", response);
    }
    #endregion

    #region Post
    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Post<TResult, TValue>(string url, TValue content) where TResult : POCO where TValue : POCO => await Post<TResult, TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Post<TValue>(string url, TValue content) where TValue : POCO => await Post<TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Post<TResult, TValue>(string url, UrlParameters urlParameters, TValue content) where TResult : POCO where TValue : POCO => await Post<TResult, TValue>(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Post<TValue>(string url, UrlParameters urlParameters, TValue content) where TValue : POCO => await Post(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Post<TResult, TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) where TResult : POCO where TValue : POCO => await Post<TResult, TValue>(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Post<TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) where TValue : POCO => await Post(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Post<TResult, TValue>(string url, TValue content, CancellationToken cancellationToken) where TResult : POCO where TValue : POCO
    {
        var payload = JsonContent.Create(content);

        Console.WriteLine("Post Issued");

        HttpResponseMessage response = await Client.PostAsync(url, payload, cancellationToken);

        return await GenerateResult<TResult>("Post", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Post<TValue>(string url, TValue content, CancellationToken cancellationToken) where TValue : POCO
    {
        var payload = JsonContent.Create(content);

        Console.WriteLine("Post Issued");

        HttpResponseMessage response = await Client.PostAsync(url, payload, cancellationToken);

        return GenerateResult("Post", response);
    }
    #endregion

    #region Put
    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Put<TResult, TValue>(string url, TValue content) where TResult : POCO where TValue : POCO => await Put<TResult, TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Put<TValue>(string url, TValue content) where TValue : POCO => await Put<TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Put<TResult, TValue>(string url, UrlParameters urlParameters, TValue content) where TResult : POCO where TValue : POCO => await Put<TResult, TValue>(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Put<TValue>(string url, UrlParameters urlParameters, TValue content) where TValue : POCO => await Put(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Put<TResult, TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) where TResult : POCO where TValue : POCO => await Put<TResult, TValue>(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Put<TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) where TValue : POCO => await Put(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Put<TResult, TValue>(string url, TValue content, CancellationToken cancellationToken) where TResult : POCO where TValue : POCO
    {
        var payload = JsonContent.Create(content);

        Console.WriteLine("Put Issued");

        HttpResponseMessage response = await Client.PutAsync(url, payload, cancellationToken);

        return await GenerateResult<TResult>("Put", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Put<TValue>(string url, TValue content, CancellationToken cancellationToken) where TValue : POCO
    {
        var payload = JsonContent.Create(content);

        Console.WriteLine("Put Issued");

        HttpResponseMessage response = await Client.PutAsync(url, payload, cancellationToken);

        return GenerateResult("Put", response);
    }
    #endregion

    #region Patch
    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Patch<TResult, TValue>(string url, TValue content) where TResult : POCO where TValue : POCO => await Patch<TResult, TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Patch<TValue>(string url, TValue content) where TValue : POCO => await Patch<TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Patch<TResult, TValue>(string url, UrlParameters urlParameters, TValue content) where TResult : POCO where TValue : POCO => await Patch<TResult, TValue>(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Patch<TValue>(string url, UrlParameters urlParameters, TValue content) where TValue : POCO => await Patch(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Patch<TResult, TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) where TResult : POCO where TValue : POCO => await Patch<TResult, TValue>(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Patch<TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) where TValue : POCO => await Patch(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Patch<TResult, TValue>(string url, TValue content, CancellationToken cancellationToken) where TResult : POCO where TValue : POCO
    {
        var payload = JsonContent.Create(content);

        Console.WriteLine("Patch Issued");

        HttpResponseMessage response = await Client.PatchAsync(url, payload, cancellationToken);

        return await GenerateResult<TResult>("Patch", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Patch<TValue>(string url, TValue content, CancellationToken cancellationToken) where TValue : POCO
    {
        var payload = JsonContent.Create(content);

        Console.WriteLine("Patch Issued");

        HttpResponseMessage response = await Client.PatchAsync(url, payload, cancellationToken);

        return GenerateResult("Patch", response);
    }
    #endregion

    #region Delete
    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Delete<TResult>(string url) where TResult : POCO => await Delete<TResult>(url, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Delete(string url) => await Delete(url, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Delete<TResult>(string url, UrlParameters urlParameters) where TResult : POCO => await Delete<TResult>(url + urlParameters.ToUrlParameters());

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Delete(string url, UrlParameters urlParameters) => await Delete(url + urlParameters.ToUrlParameters());

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Delete<TResult>(string url, UrlParameters urlParameters, CancellationToken cancellationToken) where TResult : POCO => await Delete<TResult>(url + urlParameters.ToUrlParameters(), cancellationToken);

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Delete(string url, UrlParameters urlParameters, CancellationToken cancellationToken) => await Delete(url + urlParameters.ToUrlParameters(), cancellationToken);

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Delete<TResult>(string url, CancellationToken cancellationToken) where TResult : POCO
    {
        Console.WriteLine("Delete Issued");

        HttpResponseMessage response = await Client.DeleteAsync(url, cancellationToken);

        return await GenerateResult<TResult>("Delete", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Delete(string url, CancellationToken cancellationToken)
    {
        Console.WriteLine("Delete Issued");

        HttpResponseMessage response = await Client.DeleteAsync(url, cancellationToken);

        return GenerateResult("Delete", response);
    }
    #endregion

    private static async Task<APIResponse<TResult>> GenerateResult<TResult>(string requestType, HttpResponseMessage response, CancellationToken cancellationToken) where TResult : POCO
    {
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"{requestType} succeded");
            return new APIResponse<TResult>(response, await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken));
        }
        else
        {
            Console.WriteLine($"{requestType} failed: {(int)response.StatusCode} - {response.ReasonPhrase}");
            return new APIResponse<TResult>(response);
        }
    }

    private static APIResponse GenerateResult(string requestType, HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"{requestType} succeded");
        }
        else
        {
            Console.WriteLine($"{requestType} failed: {(int)response.StatusCode} - {response.ReasonPhrase}");
        }

        return new APIResponse(response);
    }
}