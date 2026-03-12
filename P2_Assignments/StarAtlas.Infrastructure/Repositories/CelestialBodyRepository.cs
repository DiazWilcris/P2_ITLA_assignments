using Microsoft.EntityFrameworkCore;
using StarAtlas.Domain.Entities;
using StarAtlas.Persistence.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarAtlas.Infrastructure.Repositories
{
    public class CelestialBodyRepository : GenericRepository<CelestialBody>
    {
        public CelestialBodyRepository(StarAtlasContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CelestialBody>> GetAllWithTypesAsync()
        {
            return await _context.CelestialBodies
                                 .Include(c => c.BodyType)
                                 .ToListAsync();
        }

        public async Task<CelestialBody?> GetByIdWithTypeAsync(int id)
        {
            return await _context.CelestialBodies
                                 .Include(c => c.BodyType)
                                 .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}