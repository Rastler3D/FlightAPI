﻿using Application.Common.Abstractions.Caching;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Repositories;
using Infrastructure.Caching;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("flight-db");

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });


        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();
        
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("cache");
        });

        
        builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(provider =>
        {
            var connectionString = builder.Configuration.GetConnectionString("cache");
            return StackExchange.Redis.ConnectionMultiplexer.Connect(connectionString!);
        });


        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        

        builder.Services.AddSingleton(TimeProvider.System);
        
        builder.Services.AddScoped<IFlightRepository, FlightRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICacheService, CacheService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IPasswordService, PasswordService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        builder.Services.AddScoped<ApplicationDbContextInitializer>();

     

       
    }
}