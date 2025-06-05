using Milvasoft.Core.EntityBases.Concrete.Auditing;
using Milvasoft.Core.Helpers.GeoLocation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountrySeeder.Entities;

/// <summary>
/// State information. It means city for Türkiye.
/// </summary>
[Table("States")]
public class State : FullAuditableEntity<int>
{
    /// <summary>
    /// Name of state.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Iso 2 code of state.
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    /// Iso 2 code of state.
    /// </summary>
    public string StateCode { get; set; }

    /// <summary>
    /// Type of state (e.g., province, state, region).
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Geographic point of state.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public GeoPoint GeoPoint { get; set; }

    /// <summary>
    /// Country id that this state belongs to.
    /// </summary>
    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }

    /// <summary>
    /// Country that this state belongs to.
    /// </summary>
    public virtual Country Country { get; set; }

    /// <summary>
    /// List of cities that belong to this state.
    /// </summary>
    public virtual List<City> Cities { get; set; }
}
