using Domain.Enums;
// using Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Data.AppDbContext;
using Domain.Common;
using Domain.Enums.Common;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Entities.Identities;
using Microsoft.Extensions.Configuration;
using Domain.ConfigurationModels;
using Domain.Entities;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Data.Seeders.Org
{
    public static class DbInitializer
    {

        public static async Task SeedData<TEntity>(this IHost host, List<TEntity> entities) where TEntity : AuditableEntity
        {
            var serviceProvider = host.Services.CreateScope().ServiceProvider;
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            var contextEntities = context.Set<TEntity>();
            if (!contextEntities.Any())
            {
                await contextEntities.AddRangeAsync(entities);
                await context.SaveChangesAsync();
            }
        }
        public static async Task SeedSuperAdmin(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            await context.Database.EnsureCreatedAsync();

            var query = context.Set<User>().AsQueryable();
            var email = "admin@admin.com";

            var getUser = await query.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.UserName.Equals(email));

            if (getUser == null)
            {
                var newUser = new User
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    PhoneNumberConfirmed = false,
                    LockoutEnabled = false,
                    CreatedAt = DateTime.UtcNow,
                    Email = email,
                    UserName = email,
                    Verified = true,
                    IsActive = true,
                    PhoneNumber = "074444444000",
                    Role = "SUPERADMIN",
                    UpdatedAt = DateTime.UtcNow,
                };
                await userManager.CreateAsync(newUser, "Admin123@");
            }
        }

    }
}
