namespace StarAtlas.API.Models.Dtos
{
    public class ObservationDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty; 
        public string CelestialBodyName { get; set; } = string.Empty; 
    }
}