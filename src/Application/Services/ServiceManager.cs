using Application.Contracts;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Infrastructure.Data.Utils.Storage;
using Infrastructure.Data.Utils.Email;
using Application.Services;
using Domain.Entities.Identities;
using Application.Contracts.Utility;
using Application.Services.Utility;
using Infrastructure.Contracts;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Application.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEnumService> _enumService;

        public ServiceManager(
            IAzureStorage storage,
            IRepositoryManager repository,
            IMapper mapper,
            IConfiguration configuration,
            UserManager<User> userManager,
            IWebHelper webHelper,
            IEmailManager emailManager,
            RoleManager<Role> roleManager)
        {
            _enumService = new Lazy<IEnumService>(() => new EnumService());
        }

        public IEnumService EnumService => _enumService.Value;
    }
}
