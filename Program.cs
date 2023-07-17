using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//CORS
builder.Services.AddApplicationServices(builder.Configuration);
//Authentication
builder.Services.AddIdentityService(builder.Configuration);

var app = builder.Build();

//Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("https://localhost:4200"))
    ;

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//SignalR
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");


//Seed data
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    context.Connections.RemoveRange(context.Connections);
    await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
    //Seed data
    await Seed.SeedUsers(userManager, roleManager);
}
catch(Exception ex){
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating");
}

app.Run();
