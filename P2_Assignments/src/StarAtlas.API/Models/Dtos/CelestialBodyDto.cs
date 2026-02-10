namespace StarAtlas.API.Models.Dtos
{
    public class CelestialBodyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; 
        public double DistanceLightYears { get; set; }
    }
}