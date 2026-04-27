using MechanicalWorkshop.Application.Interfaces;
using MechanicalWorkshop.Application.UseCases.Auth;
using MechanicalWorkshop.Application.UseCases.Customers;
using MechanicalWorkshop.Application.UseCases.Vehicles;
using MechanicalWorkshop.Application.UseCases.Services;
using MechanicalWorkshop.Application.UseCases.Parts;
using MechanicalWorkshop.Domain.Interfaces.Repositories;
using MechanicalWorkshop.Infrastructure.Persistence;
using MechanicalWorkshop.Infrastructure.Persistence.Repositories;
using MechanicalWorkshop.Infrastructure.Security;

namespace MechanicalWorkshop.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IPartRepository, PartRepository>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();

        // Security
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // Use Cases - Auth
        services.AddScoped<LoginUseCase>();

        // Use Cases - Customers
        services.AddScoped<CreateCustomerUseCase>();
        services.AddScoped<GetCustomerUseCase>();
        services.AddScoped<UpdateCustomerUseCase>();
        services.AddScoped<DeleteCustomerUseCase>();

        // Use Cases - Vehicles
        services.AddScoped<CreateVehicleUseCase>();
        services.AddScoped<GetVehicleUseCase>();
        services.AddScoped<UpdateVehicleUseCase>();
        services.AddScoped<DeleteVehicleUseCase>();

        // Use Cases - Services
        services.AddScoped<CreateServiceUseCase>();
        services.AddScoped<GetServiceUseCase>();
        services.AddScoped<UpdateServiceUseCase>();
        services.AddScoped<DeleteServiceUseCase>();

        // Use Cases - Parts
        services.AddScoped<CreatePartUseCase>();
        services.AddScoped<GetPartUseCase>();
        services.AddScoped<UpdatePartUseCase>();
        services.AddScoped<DeletePartUseCase>();

        return services;
    }
}