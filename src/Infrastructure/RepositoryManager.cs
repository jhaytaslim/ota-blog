using Domain.Common;
using Infrastructure.Contracts;
using Infrastructure.Data.AppDbContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Domain.Helpers;
using Infrastructure.Contracts;

namespace Infrastructure;
public class RepositoryManager : IRepositoryManager
{
    private static IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _appDbContext;

    public RepositoryManager(ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _appDbContext = appDbContext;
    }

    public async Task BeginTransaction(Func<Task> action)
    {
        await using var transaction = await _appDbContext.Database.BeginTransactionAsync();
        try
        {
            await action();

            await SaveChangesAsync();
            await transaction.CommitAsync();

        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            TrackAddedEntities();
            TrackModifiedEntities();
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}: \n {ex.StackTrace}");
            throw;
        }
    }

    private void TrackAddedEntities()
    {
        var entries = _appDbContext.ChangeTracker.Entries<IAuditableEntity>().Where(e => e.State == EntityState.Added);
        var createdById = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypeHelper.UserId).FirstOrDefault()?.Value ?? null;
        foreach (var entry in entries)
        {
            entry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
            entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
            entry.Property(x => x.CreatedById).CurrentValue = createdById;

            if (entry.Property(x => x.Id).CurrentValue == Guid.Empty)
                entry.Property(x => x.Id).CurrentValue = Guid.NewGuid();
        }
    }

    private void TrackModifiedEntities()
    {
        var entries = _appDbContext.ChangeTracker.Entries<IAuditableEntity>().Where(e => e.State == EntityState.Modified);
        foreach (var entry in entries)
        {
            entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
        }
    }

}

