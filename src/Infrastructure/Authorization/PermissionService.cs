using Infrastructure.Contracts;
using Newtonsoft.Json;

namespace Infrastructure.Authorization;

public class PermissionService : IPermissionService
{
    private readonly ISessionRepository _sessionRepository;
    public PermissionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<List<string>?> GetPermissionsAsync(string email)
    {
        var userSession = await _sessionRepository
            .FirstOrDefaultAsync(x => x.Email == email);

        if (userSession == null)
        {
            return null;
        }

        List<string>? permissions = JsonConvert.DeserializeObject<List<string>>(userSession.FeatureList);

        return permissions;
    }
}