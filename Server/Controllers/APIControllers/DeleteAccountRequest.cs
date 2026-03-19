using System.Text.Json.Serialization;

namespace Server.Controllers.APIControllers;

public class DeleteAccountRequest
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    // MobileID uses the same casing as the MobileApp JSON payloads (mobileID).
    [JsonPropertyName("mobileID")]
    public int? MobileID { get; set; }
}

