using Infrastructure.Data;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using WebApp.ExtensionMethods.AuthConfig;
using WebApp.ExtensionMethods.RegisterService;
using WebApp.ExtensionMethods.SwaggerConfig;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRegisterService(builder.Configuration);


// register swagger configuration
builder.Services.SwaggerService();

// authentications service
builder.Services.AddAuthConfigureService(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


// update database
try
{
    var serviceProvider = app.Services.CreateScope().ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();

    //seed data
    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.Initial();
}
catch (Exception)
{
    // ignored
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();