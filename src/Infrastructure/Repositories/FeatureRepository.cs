using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure.Data.AppDbContext;

namespace Infrastructure.Repositories;

public class FeatureRepository : Repository<Feature>, IFeatureRepository
{
    public FeatureRepository(ApplicationDbContext appDbContext) : base(appDbContext)
    {
    }

    public IQueryable<Feature> GetAllFeaturesQuery(string search = null)
    {
        var query = _context.Features.OrderByDescending(x => x.CreatedAt) as IQueryable<Feature>;

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            var searchQuery = search.Trim();
            query = query.Where(x =>
                (x.Name.Contains(searchQuery))
                || (!string.IsNullOrWhiteSpace(x.Group) && x.Group.ToLower().Contains(searchQuery))
            );
        }
        return query;
    }


}