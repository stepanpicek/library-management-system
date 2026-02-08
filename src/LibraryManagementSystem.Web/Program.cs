using LibraryManagementSystem.Application;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Infrastructure;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Web;
using LibraryManagementSystem.Web.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();


await using (var scope = app.Services.CreateAsyncScope())
await using (var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
{
    await db.Database.EnsureCreatedAsync();
}

app.UseExceptionHandler("/Error", createScopeForErrors: true);
app.UseHttpsRedirection();

app.MapOpenApi();
app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "v1"));

app.MapControllers();

app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.Run();

public partial class Program { }
