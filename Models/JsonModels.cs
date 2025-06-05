using System.Text.Json.Serialization;

namespace CountrySeeder.Models;

public class CountryModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("iso3")]
    public string Iso3 { get; set; }

    [JsonPropertyName("iso2")]
    public string Iso2 { get; set; }

    [JsonPropertyName("numeric_code")]
    public string NumericCode { get; set; }

    [JsonPropertyName("phonecode")]
    public string PhoneCode { get; set; }

    [JsonPropertyName("capital")]
    public string Capital { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("currency_name")]
    public string CurrencyName { get; set; }

    [JsonPropertyName("currency_symbol")]
    public string CurrencySymbol { get; set; }

    [JsonPropertyName("tld")]
    public string Tld { get; set; }

    [JsonPropertyName("native")]
    public string Native { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("subregion")]
    public string Subregion { get; set; }

    [JsonPropertyName("timezones")]
    public List<TimezoneModel> Timezones { get; set; }

    [JsonPropertyName("translations")]
    public Dictionary<string, string> Translations { get; set; }

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; }

    [JsonPropertyName("emoji")]
    public string Emoji { get; set; }

    [JsonPropertyName("emojiU")]
    public string EmojiU { get; set; }

    [JsonPropertyName("states")]
    public List<StateModel> States { get; set; }
}

public class TimezoneModel
{
    [JsonPropertyName("zoneName")]
    public string ZoneName { get; set; }

    [JsonPropertyName("gmtOffset")]
    public int GmtOffset { get; set; }

    [JsonPropertyName("gmtOffsetName")]
    public string GmtOffsetName { get; set; }

    [JsonPropertyName("abbreviation")]
    public string Abbreviation { get; set; }

    [JsonPropertyName("tzName")]
    public string TzName { get; set; }
}

public class StateModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("state_code")]
    public string StateCode { get; set; }

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("cities")]
    public List<CityModel> Cities { get; set; }
}

public class CityModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; }
}
