using System.Net;

namespace Client;

public class APIRequestErrorHandler
{
    private readonly List<(Func<APIResponse, bool> predicate, Func<Task> action)> ErrorHandlers = new();

    public APIRequestErrorHandler AddCondition(HttpStatusCode statusCode, Func<Task> action) => AddCondition((response) => response.StatusCode == statusCode, action);

    public APIRequestErrorHandler AddCondition(Func<APIResponse, bool> predicate, Func<Task> action)
    {
        ErrorHandlers.Add((predicate, action));
        return this;
    }

    public Func<Task>? Find(APIResponse response)
    {
        foreach ((Func<APIResponse, bool> predicate, Func<Task> action) in ErrorHandlers)
        {
            if (predicate.Invoke(response))
            {
                return action;
            }
        }

        return null;
    }
}
