using LibraryManagementSystem.Application;
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

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await db.Database.MigrateAsync();

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
