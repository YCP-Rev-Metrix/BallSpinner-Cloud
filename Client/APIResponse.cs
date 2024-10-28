using Common.POCOs;
using System.Net;

namespace Client;

/// <summary>
/// Response from API request with a POCO result
/// </summary>
/// <typeparam name="TResult">POCO expected to be returned by API request</typeparam>
public class APIResponse<TResult> : APIResponse where TResult : POCO
{
    /// <summary>
    /// Result from API request. Null if invalid
    /// </summary>
    public TResult? Result { private set; get; }

    /// <summary>
    /// If <see cref="Result"/> is valid
    /// </summary>
    public bool IsValid => WasSuccessful && Result != null;

    public APIResponse(HttpResponseMessage httpResponse, TResult? result) : base(httpResponse) => Result = result;

    public APIResponse(HttpResponseMessage httpResponse) : base(httpResponse) => Result = default;
}

/// <summary>
/// Response from API request
/// </summary>
public class APIResponse
{
    /// <summary>
    /// The HttpResponse resulting from the API request
    /// </summary>
    public HttpResponseMessage HttpResponse { private set; get; }

    /// <summary>
    /// The StatusCode resulting from the API request
    /// </summary>
    public HttpStatusCode StatusCode => HttpResponse.StatusCode;

    /// <summary>
    /// If <see cref="StatusCode"/> represents a success
    /// </summary>
    public bool WasSuccessful => HttpResponse.IsSuccessStatusCode;

    public APIResponse(HttpResponseMessage httpResponse) => HttpResponse = httpResponse;
}