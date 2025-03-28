using System.Reflection;
using Application.Services;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.Repository;

public static class AddRepositoryExtension
{
    public static IServiceCollection AddRepository(this IServiceCollection services, Assembly? assembly = null, ServiceLifetime leftTime = ServiceLifetime.Scoped)
        => services.AddScanService(typeof(IRepository<>), assembly, leftTime);  
}
