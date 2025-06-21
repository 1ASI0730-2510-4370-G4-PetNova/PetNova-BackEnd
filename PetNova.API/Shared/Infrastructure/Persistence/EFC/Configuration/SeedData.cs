using Microsoft.AspNetCore.Identity;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Status.Domain.Model.Aggregate;

namespace PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Seed initial statuses if none exist
        if (!context.Statuses.Any())
        {
            var statuses = new List<Status>
            {
                new() { Name = "Scheduled", Description = "Appointment is scheduled", Type = "Appointment" },
                new() { Name = "Completed", Description = "Appointment is completed", Type = "Appointment" },
                new() { Name = "Cancelled", Description = "Appointment is cancelled", Type = "Appointment" },
                new() { Name = "Active", Description = "Pet is active", Type = "Pet" },
                new() { Name = "Inactive", Description = "Pet is inactive", Type = "Pet" }
            };
            
            await context.Statuses.AddRangeAsync(statuses);
        }

        // Seed admin user if none exists
        if (!context.Users.Any())
        {
            var passwordHasher = new PasswordHasher<User>();
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@petnova.com",
                Role = "Admin"
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");
            
            await context.Users.AddAsync(adminUser);
        }

        await context.SaveChangesAsync();
    }
}
