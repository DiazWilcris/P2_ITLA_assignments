using System.ComponentModel.DataAnnotations;

namespace StarAtlas.Application.Dtos.Observations
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