using StarAtlas.Domain.Entities;
using StarAtlas.Persistence.Context;
using System.Threading.Tasks;

namespace StarAtlas.Infrastructure.Repositories
{
    public class UnitOfWork
    {
        private readonly StarAtlasContext _context;

        public CelestialBodyRepository CelestialBodyRepository { get; set; }
        public ObservationRepository ObservationRepository { get; set; }
        public BodyTypeRepository BodyTypeRepository { get; set; } 

        public UnitOfWork(
            StarAtlasContext context,
            CelestialBodyRepository celestialBodyRepository,
            ObservationRepository observationRepository,
            BodyTypeRepository bodyTypeRepository) 
        {
            this._context = context;
            this.CelestialBodyRepository = celestialBodyRepository;
            this.ObservationRepository = observationRepository;
            this.BodyTypeRepository = bodyTypeRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
        public async Task CommitTransactionAsync() => await _context.Database.CommitTransactionAsync();
        public async Task RollbackTransactionAsync() => await _context.Database.RollbackTransactionAsync();
    }
}