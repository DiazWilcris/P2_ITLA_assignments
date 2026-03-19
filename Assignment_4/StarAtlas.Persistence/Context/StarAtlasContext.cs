using Microsoft.EntityFrameworkCore;
using StarAtlas.Domain.Entities;

namespace StarAtlas.Persistence.Context
{
    public class StarAtlasContext : DbContext
    {
        public StarAtlasContext(DbContextOptions<StarAtlasContext> options) : base(options) { }

        public DbSet<CelestialBody> CelestialBodies { get; set; }
        public DbSet<BodyType> BodyTypes { get; set; }
        public DbSet<Observation> Observations { get; set; }
    }
}