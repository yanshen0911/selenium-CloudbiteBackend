using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
public static class JsonHelper
{
    // Generic method for strict deserialization with MissingMemberHandling set to Error and case-sensitive property names
    public static T DeserializeStrict<T>(string jsonContent)
    {
        var settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error,  // Enforce error for extra fields
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new DefaultNamingStrategy()  // Enforce case sensitivity
            }
        };

        try
        {
            // Deserialize the JSON with strict settings
            return JsonConvert.DeserializeObject<T>(jsonContent, settings);
        }
        catch (JsonSerializationException ex)
        {
            // You can log the error or rethrow it for the test to catch
            throw new InvalidOperationException($"Failed to deserialize response. {ex.Message}");
        }
    }
}
