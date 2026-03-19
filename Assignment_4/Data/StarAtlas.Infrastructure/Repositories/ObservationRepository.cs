using Microsoft.EntityFrameworkCore;
using StarAtlas.Domain.Entities;
using StarAtlas.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarAtlas.Infrastructure.Repositories
{
    public class ObservationRepository : GenericRepository<Observation>
    {
        public ObservationRepository(StarAtlasContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Observation>> GetObservationsByStarAsync(int celestialBodyId)
        {
            return await _context.Observations
                                 .Include(o => o.CelestialBody)
                                 .Where(o => o.CelestialBodyId == celestialBodyId)
                                 .OrderByDescending(o => o.ObservationDate)
                                 .ToListAsync();
        }
    }
}