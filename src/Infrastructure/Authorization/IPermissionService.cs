namespace Infrastructure.Authorization;

public interface IPermissionService
{
    Task<List<string>?> GetPermissionsAsync(string staffId);
}