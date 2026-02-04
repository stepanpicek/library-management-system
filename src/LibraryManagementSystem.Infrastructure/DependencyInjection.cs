using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Database");
        builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((_, options) =>
        {
            options.UseSqlite(connectionString);
        });
    }
}