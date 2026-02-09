using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarAtlas.API.Models.Entities
{
    public class CelestialBody
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double DistanceLightYears { get; set; }
        public DateTime? DiscoveryDate { get; set; }

        public int BodyTypeId { get; set; }

        [ForeignKey("BodyTypeId")]
        public virtual BodyType? BodyType { get; set; }
    }
}