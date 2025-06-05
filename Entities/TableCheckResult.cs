namespace CountrySeeder.Entities;

/// <summary>
/// Result model for checking table existence.
/// </summary>
public class TableCheckResult
{
    [System.Text.Json.Serialization.JsonPropertyName("table_name")]
    public string TableName { get; set; }
}