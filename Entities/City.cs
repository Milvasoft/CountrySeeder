using Milvasoft.Core.EntityBases.Concrete.Auditing;
using Milvasoft.Core.Helpers.GeoLocation.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountrySeeder.Entities;

/// <summary>
/// State information. It means city for Türkiye.
/// </summary>
[Table("Cities")]
public class City : FullAuditableEntity<int>
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
    /// Geographic point of city.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public GeoPoint GeoPoint { get; set; }

    /// <summary>
    /// State id that this city belongs to.
    /// </summary>
    [ForeignKey(nameof(this.State))]
    public int StateId { get; set; }

    /// <summary>
    /// City that this city belongs to.
    /// </summary>
    public virtual State State { get; set; }
}
