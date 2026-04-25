using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MechanicalWorkshop.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        await SeedAdminUserAsync(context);
        await SeedServicesAsync(context);
        await SeedPartsAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedAdminUserAsync(AppDbContext context)
    {
        if (await context.AdminUsers.AnyAsync()) return;

        var admin = new AdminUser(
            name: "Administrator",
            email: "admin@workshop.com",
            passwordHash: BCrypt.Net.BCrypt.HashPassword("Admin@123")
        );

        await context.AdminUsers.AddAsync(admin);
    }

    private static async Task SeedServicesAsync(AppDbContext context)
    {
        if (await context.Services.AnyAsync()) return;

        var services = new List<Service>
        {
            new("Oil Change", "Complete engine oil change with filter replacement", 89.90m, 30),
            new("Wheel Alignment", "4-wheel computerized alignment", 129.90m, 60),
            new("Tire Rotation", "Rotation and balancing of all 4 tires", 79.90m, 45),
            new("Brake Inspection", "Full brake system inspection", 59.90m, 30),
            new("Brake Pad Replacement", "Front or rear brake pad replacement", 199.90m, 90),
            new("Air Filter Replacement", "Engine air filter replacement", 49.90m, 20),
            new("Battery Check", "Battery and charging system diagnostic", 39.90m, 20),
            new("Full Inspection", "Complete 50-point vehicle inspection", 149.90m, 120)
        };

        await context.Services.AddRangeAsync(services);
    }

    private static async Task SeedPartsAsync(AppDbContext context)
    {
        if (await context.Parts.AnyAsync()) return;

        var parts = new List<Part>
        {
            new("Engine Oil 5W30 1L", "Synthetic engine oil 5W30 1 liter", 35.90m, 50, 10),
            new("Oil Filter", "Universal oil filter", 25.90m, 30, 5),
            new("Air Filter", "Engine air filter", 45.90m, 20, 5),
            new("Front Brake Pad Set", "Front brake pad set (4 pieces)", 89.90m, 15, 3),
            new("Rear Brake Pad Set", "Rear brake pad set (4 pieces)", 79.90m, 15, 3),
            new("Spark Plug", "Iridium spark plug", 39.90m, 40, 8),
            new("Cabin Air Filter", "Cabin air conditioning filter", 35.90m, 20, 4),
            new("Windshield Wiper Blade", "Front windshield wiper blade", 29.90m, 25, 5)
        };

        await context.Parts.AddRangeAsync(parts);
    }
}