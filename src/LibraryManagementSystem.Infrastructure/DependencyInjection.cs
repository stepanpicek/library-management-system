using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Infrastructure.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LibraryManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));
        
        var connectionString = builder.Configuration.GetConnectionString("Database");
        builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((services, options) =>
        {
            options.UseSqlite(connectionString);
            
            var settings = services.GetRequiredService<IOptions<DatabaseSettings>>();
            
            options.UseSeeding((context, _) =>
            {
                var booksExists = context.Set<Book>().Any();
                if (!booksExists && settings.Value.UseSeeding)
                {
                    BookSeeds.SeedBooks(context);
                    context.SaveChanges();
                }
            });
            
            options.UseAsyncSeeding(async (context, _, ctx) =>
            {
                var booksExists = context.Set<Book>().Any();
                if (!booksExists && settings.Value.UseSeeding)
                {
                    await BookSeeds.SeedBooksAsync(context);
                    await context.SaveChangesAsync(ctx);
                }
            });
        });
        
    }
}