using System.ComponentModel.DataAnnotations;

namespace StarAtlas.Domain.Entities
{
    public class BodyType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}