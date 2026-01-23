namespace Server.Controllers.DatabaseControllers2025.Video
{
    using global::Server.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Server.Services;

    [ApiController]
    [Route("api/videos")]
    public class VideoController : ControllerBase
    {
        private readonly SpacesVideoService _spaces;

        public VideoController(SpacesVideoService spaces)
        {
            _spaces = spaces;
        }

        // Upload video: multipart/form-data with field "file"
        [Authorize]
        [HttpPost("upload")]
        [RequestSizeLimit(500 * 1024 * 1024)] // 500MB
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromQuery] string? folder)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var key = $"{(string.IsNullOrWhiteSpace(folder) ? "app" : folder.TrimEnd('/'))}/{Guid.NewGuid()}_{file.FileName}";
            await using var stream = file.OpenReadStream();

            try
            {
                await _spaces.UploadAsync(key, stream, file.ContentType);
                return Ok(new { key });
            }
            catch (Exception ex)
            {
                return Problem($"Upload failed: {ex.Message}");
            }
        }

        // Generate pre-signed download URL
        [Authorize]
        [HttpGet("presign")]
        public async Task<IActionResult> GetPresignedUrl([FromQuery] string key, [FromQuery] int ttlSeconds = 900)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest("Missing key.");
            if (!await _spaces.ExistsAsync(key))
                return NotFound("Video not found.");

            var url = _spaces.GetPresignedUrl(key, TimeSpan.FromSeconds(ttlSeconds));
            return Ok(new { url, expiresIn = ttlSeconds });
        }

        // Optional: proxy-download for controlled streaming (less scalable; prefer presigned URLs)
        [Authorize]
        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest("Missing key.");
            if (!await _spaces.ExistsAsync(key))
                return NotFound();

            var stream = await _spaces.GetObjectStreamAsync(key);
            return File(stream, "application/octet-stream", enableRangeProcessing: true);
        }
    }
}
