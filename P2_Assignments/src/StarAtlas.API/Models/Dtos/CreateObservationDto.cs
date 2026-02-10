using System.ComponentModel.DataAnnotations;

namespace StarAtlas.API.Models.Dtos
{
    public class CreateObservationDto
    {
        [Required]
        public int CelestialBodyId { get; set; }

        [Required]
        public string PersonalNote { get; set; } = string.Empty;

        public string? Location { get; set; }
    }
}