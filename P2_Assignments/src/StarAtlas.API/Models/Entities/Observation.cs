using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarAtlas.API.Models.Entities
{
    public class Observation
    {
        [Key]
        public int Id { get; set; }
        public DateTime ObservationDate { get; set; } = DateTime.Now;
        public string? Location { get; set; }
        public string PersonalNote { get; set; } = string.Empty;

        public int CelestialBodyId { get; set; }

        [ForeignKey("CelestialBodyId")]
        public virtual CelestialBody? CelestialBody { get; set; }
    }
}