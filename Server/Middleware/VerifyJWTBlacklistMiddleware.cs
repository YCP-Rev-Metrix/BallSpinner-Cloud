using Microsoft.AspNetCore.Authorization;

namespace Server.Middleware;

/// <summary>
///<see cref="IMiddleware"/> to verify that the the request's JWT is not in the blacklist via <see cref="Security.Stores.AbstractTokenStore"/>
/// </summary>
public class VerifyJWTBlacklistMiddleware : IMiddleware
{
    /// <inheritdoc/>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Get the expected endpoint
        Endpoint? endpoint = context.GetEndpoint();

        if (endpoint != null)
        {
            // Only worry about the validity of the JWT if the endpoint cares about authorization
            if (endpoint.Metadata.GetMetadata<AuthorizeAttribute>() != null)
            {
                // Get JWT from the request headers
                string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (!string.IsNullOrEmpty(token))
                {
                    // Actually check blacklist
                    if (!ServerState.TokenStore.IsAuthorizationBlacklisted(token))
                    {
                        // Invoke the next item (likely the endpoint)
                        await next.Invoke(context);
                        return;
                    }
                }

                // Return Unauthorized if token is blacklisted
                context.Response.StatusCode = 401;
                return;
            }
        }

        // Invoke the next item (likely the endpoint)
        await next.Invoke(context);
    }
}
