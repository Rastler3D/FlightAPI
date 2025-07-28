using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
    }
}

public sealed class ApplicationDbContextInitializer(
    ILogger<ApplicationDbContextInitializer> logger,
    ApplicationDbContext context,
    IPasswordService passwordService)
{
    public async Task InitialiseAsync()
    {
        try
        {
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync();
            }
            else
            {
                
                await context.Database.EnsureCreatedAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        await SeedRolesAsync();
        
        await SeedUsersAsync();
        
        await SeedFlightsAsync();
    }

    private async Task SeedRolesAsync()
    {
        // Default roles
        var userRole = new Role(Roles.User);
        var moderatorRole = new Role(Roles.Moderator);

        if (!await context.Roles.AnyAsync(r => r.Code == Roles.User))
        {
            context.Roles.Add(userRole);
            logger.LogInformation("Seeded User role");
        }

        if (!await context.Roles.AnyAsync(r => r.Code == Roles.Moderator))
        {
            context.Roles.Add(moderatorRole);
            logger.LogInformation("Seeded Moderator role");
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        var userRoleId = await context.Roles
            .Where(r => r.Code == Roles.User)
            .Select(r => r.Id)
            .FirstAsync();

        var moderatorRoleId = await context.Roles
            .Where(r => r.Code == Roles.Moderator)
            .Select(r => r.Id)
            .FirstAsync();

        
        const string defaultPassword = "password123";
        
        var hashedPassword = passwordService.HashPassword(defaultPassword);
        
        var regularUser = new User("user", hashedPassword, userRoleId);
        if (!await context.Users.AnyAsync(u => u.Username == regularUser.Username))
        {
            context.Users.Add(regularUser);
            logger.LogInformation("Seeded regular user: {Username}", regularUser.Username);
        }
        
        var moderatorUser = new User("moderator", hashedPassword, moderatorRoleId);
        if (!await context.Users.AnyAsync(u => u.Username == moderatorUser.Username))
        {
            context.Users.Add(moderatorUser);
            logger.LogInformation("Seeded moderator user: {Username}", moderatorUser.Username);
        }
        
        var adminUser = new User("admin", hashedPassword, moderatorRoleId);
        if (!await context.Users.AnyAsync(u => u.Username == adminUser.Username))
        {
            context.Users.Add(adminUser);
            logger.LogInformation("Seeded admin user: {Username}", adminUser.Username);
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedFlightsAsync()
    {
        // Seed sample flights if none exist
        if (!await context.Flights.AnyAsync())
        {
            var sampleFlights = new[]
            {
                Flight.Create(
                    "New York (JFK)",
                    "Los Angeles (LAX)",
                    new DateTimeOffset(2024, 12, 15, 8, 0, 0, TimeSpan.FromHours(-5)), // EST
                    new DateTimeOffset(2024, 12, 15, 11, 30, 0, TimeSpan.FromHours(-8)), // PST
                    FlightStatus.InTime
                ),
                Flight.Create(
                    "Chicago (ORD)",
                    "Miami (MIA)",
                    new DateTimeOffset(2024, 12, 15, 10, 15, 0, TimeSpan.FromHours(-6)), // CST
                    new DateTimeOffset(2024, 12, 15, 14, 45, 0, TimeSpan.FromHours(-5)), // EST
                    FlightStatus.InTime
                ),
                Flight.Create(
                    "San Francisco (SFO)",
                    "Seattle (SEA)",
                    new DateTimeOffset(2024, 12, 15, 12, 30, 0, TimeSpan.FromHours(-8)), // PST
                    new DateTimeOffset(2024, 12, 15, 15, 0, 0, TimeSpan.FromHours(-8)), // PST
                    FlightStatus.Delayed
                ),
                Flight.Create(
                    "Boston (BOS)",
                    "Denver (DEN)",
                    new DateTimeOffset(2024, 12, 15, 14, 0, 0, TimeSpan.FromHours(-5)), // EST
                    new DateTimeOffset(2024, 12, 15, 16, 30, 0, TimeSpan.FromHours(-7)), // MST
                    FlightStatus.InTime
                ),
                Flight.Create(
                    "Atlanta (ATL)",
                    "Phoenix (PHX)",
                    new DateTimeOffset(2024, 12, 15, 16, 45, 0, TimeSpan.FromHours(-5)), // EST
                    new DateTimeOffset(2024, 12, 15, 18, 15, 0, TimeSpan.FromHours(-7)), // MST
                    FlightStatus.Cancelled
                ),
                Flight.Create(
                    "Dallas (DFW)",
                    "New York (LGA)",
                    new DateTimeOffset(2024, 12, 15, 18, 30, 0, TimeSpan.FromHours(-6)), // CST
                    new DateTimeOffset(2024, 12, 15, 23, 15, 0, TimeSpan.FromHours(-5)), // EST
                    FlightStatus.InTime
                ),
                Flight.Create(
                    "Las Vegas (LAS)",
                    "Orlando (MCO)",
                    new DateTimeOffset(2024, 12, 15, 20, 0, 0, TimeSpan.FromHours(-8)), // PST
                    new DateTimeOffset(2024, 12, 16, 3, 45, 0, TimeSpan.FromHours(-5)), // EST (next day)
                    FlightStatus.Delayed
                ),
                Flight.Create(
                    "Houston (IAH)",
                    "San Francisco (SFO)",
                    new DateTimeOffset(2024, 12, 16, 6, 15, 0, TimeSpan.FromHours(-6)), // CST
                    new DateTimeOffset(2024, 12, 16, 8, 30, 0, TimeSpan.FromHours(-8)), // PST
                    FlightStatus.InTime
                )
            };

            foreach (var flight in sampleFlights)
            {
                flight.ClearDomainEvents();
                context.Flights.Add(flight);
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} sample flights", sampleFlights.Length);
        }
    }
}
