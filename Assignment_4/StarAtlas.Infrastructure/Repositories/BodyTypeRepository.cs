using StarAtlas.Domain.Entities;
using StarAtlas.Persistence.Context;

namespace StarAtlas.Infrastructure.Repositories
{
    public class BodyTypeRepository : GenericRepository<BodyType>
    {
        public BodyTypeRepository(StarAtlasContext context) : base(context)
        {
        }

    }
}