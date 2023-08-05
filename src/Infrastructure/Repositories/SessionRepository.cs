using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure.Data.AppDbContext;

namespace Infrastructure.Repositories;
public class SessionRepository : Repository<Session>, ISessionRepository
{
    public SessionRepository(ApplicationDbContext context) : base(context)
    {
    }
}

