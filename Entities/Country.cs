using Milvasoft.Core.EntityBases.Concrete.Auditing;
using Milvasoft.Core.Helpers.GeoLocation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountrySeeder.Entities;

/// <summary>
/// Country information. Based on https://github.com/dr5hn/countries-states-cities-database
/// </summary>
[Table("Countries")]
public class Country : FullAuditableEntity<int>
{
    /// <summary>
    /// Name of country.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Iso code of country.
    /// </summary>
    public string Iso3 { get; set; }

    /// <summary>
    /// Iso 2 code of country.
    /// </summary>
    public string Iso2 { get; set; }

    /// <summary>
    /// Iso 2 code of country.
    /// </summary>
    public string NumericCode { get; set; }

    /// <summary>
    /// Iso 2 code of country.
    /// </summary>
    public string PhoneCode { get; set; }

    /// <summary>
    /// Iso 2 code of country.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Iso 2 code of country.
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// Native name of country.
    /// </summary>
    public string Native { get; set; }

    /// <summary>
    /// Geographic point of country.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public GeoPoint GeoPoint { get; set; }

    /// <summary>
    /// Iso 2 code of country.
    /// </summary>
    public string FlagEmoji { get; set; }

    /// <summary>
    /// Iso 2 code of country.
    /// </summary>
    public string FlagEmojiU { get; set; }

    /// <summary>
    /// Iso 2 code of country.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public Dictionary<string, string> Timezones { get; set; }

    /// <summary>
    /// Iso 2 code of country.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public Dictionary<string, string> Translations { get; set; }

    /// <summary>
    /// List of states (or provinces) in the country.
    /// </summary>
    public virtual List<State> States { get; set; }
}
