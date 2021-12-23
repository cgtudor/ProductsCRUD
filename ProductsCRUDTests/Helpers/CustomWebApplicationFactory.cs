﻿using System;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductsCRUD;
using ProductsCRUD.AutomatedCacher.Concrete;
using ProductsCRUD.AutomatedCacher.Interface;
using ProductsCRUD.AutomatedCacher.Model;
using ProductsCRUD.Context;
using ProductsCRUD.DomainModels;
using ProductsCRUD.Repositories.Concrete;
using ProductsCRUD.Repositories.Interface;

namespace ProductsCRUDTests.Helpers
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                // Add a database context (AppDbContext) using an in-memory database for testing.
                services.AddDbContext<Context>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryAppDb");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var appDb = scopedServices.GetRequiredService<Context>();

                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                    // Ensure the database is created.
                    appDb.Database.EnsureCreated();
                }
            }).UseEnvironment("Integration");
        }
    }
}
