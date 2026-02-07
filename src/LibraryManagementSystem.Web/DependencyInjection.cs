using LibraryManagementSystem.Web.Infrastructure;
using LibraryManagementSystem.Web.Services;

namespace LibraryManagementSystem.Web;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        builder.Services.AddScoped<IToastService, ToastService>();
        builder.Services.AddScoped<IActionRunner, ActionRunner>();
    }
}