namespace Client;

/// <summary>
/// Class handling Url parameters to avoid erros in the url
/// </summary>
public class UrlParameters
{
    /// <summary>
    /// List of url parameters
    /// </summary>
    public List<(string key, string value)>? Parameters { get; private set; } = null;

    /// <summary>
    /// Get a new, empty <see cref="UrlParameters"/> object
    /// </summary>
    public static UrlParameters Empty => new();

    public UrlParameters() { }

    public UrlParameters(params (string key, string value)[] parameters)
    {
        Parameters = new();
        Parameters.AddRange(parameters);
    }

    /// <summary>
    /// Add key value pairs to the url parameters
    /// </summary>
    /// <param name="parameters">Comma seperated (<see cref="string"/> key, <see cref="string"/> value) url parameters</param>
    /// <returns>This <see cref="UrlParameters"/> object - acting as a builder</returns>
    public UrlParameters Add(params (string key, string value)[] parameters)
    {
        Parameters ??= new();

        Parameters.AddRange(parameters);

        return this;
    }

    /// <summary>
    /// Creates a url acceptable string representing this <see cref="UrlParameters"/> object
    /// </summary>
    /// <returns>url acceptable string query - append to end of url</returns>
    public string ToUrlParameters()
    {
        string urlExtension = "";
        if (Parameters != null)
        {
            urlExtension = "?";
            foreach ((string key, string value) in Parameters)
            {
                urlExtension += $"{key}={value}&";
            }
            urlExtension = urlExtension.Remove(urlExtension.Length - 1);
        }

        return urlExtension;
    }
}
