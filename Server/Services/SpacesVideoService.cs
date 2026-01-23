using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace Server.Services;

public class SpacesVideoService
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucket;

    public SpacesVideoService(IConfiguration config)
    {
        var endpoint = config["Spaces:Endpoint"];    // e.g., https://sfo3.digitaloceanspaces.com
        var region = config["Spaces:Region"];        // must be "us-east-1"
        var accessKey = config["Spaces:AccessKey"];
        var secretKey = config["Spaces:SecretKey"];
        _bucket = config["Spaces:BucketName"];

        // Validate config early (do not log secrets)
        if (string.IsNullOrWhiteSpace(endpoint)) throw new InvalidOperationException("Spaces endpoint is missing.");
        if (string.IsNullOrWhiteSpace(region)) throw new InvalidOperationException("Spaces region is missing.");
        if (string.IsNullOrWhiteSpace(accessKey)) throw new InvalidOperationException("Spaces access key is missing.");
        if (string.IsNullOrWhiteSpace(secretKey)) throw new InvalidOperationException("Spaces secret key is missing.");
        if (string.IsNullOrWhiteSpace(_bucket)) throw new InvalidOperationException("Spaces bucket name is missing.");

        var s3Config = new AmazonS3Config
        {
            ServiceURL = endpoint,             // Regional endpoint only
            ForcePathStyle = true,
            //SignatureVersion = "v4",
            RegionEndpoint = RegionEndpoint.GetBySystemName(region) // use us-east-1 for Spaces
        };

        _s3 = new AmazonS3Client(accessKey, secretKey, s3Config);
    }

    public async Task UploadAsync(string key, Stream content, string contentType, CancellationToken ct = default)
    {
        var put = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = key,
            InputStream = content,
            ContentType = contentType,
            CannedACL = S3CannedACL.Private
        };

        var resp = await _s3.PutObjectAsync(put, ct);
        if ((int)resp.HttpStatusCode >= 300)
            throw new InvalidOperationException($"Upload failed: {resp.HttpStatusCode}");
    }

    public string GetPresignedUrl(string key, TimeSpan ttl)
    {
        var req = new GetPreSignedUrlRequest
        {
            BucketName = _bucket,
            Key = key,
            Expires = DateTime.UtcNow.Add(ttl),
            Verb = HttpVerb.GET
        };
        return _s3.GetPreSignedURL(req);
    }

    public async Task<Stream> GetObjectStreamAsync(string key, CancellationToken ct = default)
    {
        var resp = await _s3.GetObjectAsync(_bucket, key, ct);
        return resp.ResponseStream;
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken ct = default)
    {
        try
        {
            var resp = await _s3.GetObjectMetadataAsync(_bucket, key, ct);
            return true;
        }
        catch (AmazonS3Exception e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}