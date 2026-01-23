using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace Server.Services;

public class SpacesVideoService
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucket;

    public SpacesVideoService(IConfiguration config)
    {
        var endpoint = config["Spaces:Endpoint"];
        var region = config["Spaces:Region"];
        var accessKey = config["Spaces:AccessKey"];
        var secretKey = config["Spaces:SecretKey"];
        _bucket = config["Spaces:BucketName"];

        var s3Config = new AmazonS3Config
        {
            ServiceURL = endpoint,       // DO Spaces endpoint
            ForcePathStyle = true,       // required for some Spaces regions
            //SignatureVersion = "v4",
            RegionEndpoint = RegionEndpoint.GetBySystemName(region)
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
            CannedACL = S3CannedACL.Private // keep private; use pre-signed URLs for access
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