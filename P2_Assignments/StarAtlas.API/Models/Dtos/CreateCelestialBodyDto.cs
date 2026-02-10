using System.ComponentModel.DataAnnotations;

namespace StarAtlas.API.Models.Dtos
{
    public class CreateCelestialBodyDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double DistanceLightYears { get; set; }

        public DateTime DiscoveryDate { get; set; }

        [Required]
        public int BodyTypeId { get; set; } 
    }
}