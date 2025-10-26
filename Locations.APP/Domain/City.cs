using System.ComponentModel.DataAnnotations;
using CORE.APP.Domain;

namespace Locations.APP.Domain;

public class City : Entity
{
    [Required, StringLength(175)]
    public string CityName { get; set; }
    public int CountryId { get; set; }
    public Country Country { get; set; }
}