using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RuneFlipper.Server.DAL;
using RuneFlipper.Server.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var connectionString = builder.Configuration.GetConnectionString("RuneFlipperDb");

var connectionStringBuilder = new NpgsqlConnectionStringBuilder(
    builder.Configuration.GetConnectionString("RuneFlipperDb"));

var connectionString = connectionStringBuilder.ConnectionString;

builder.Services.AddDbContext<RuneFlipperContext>(options =>
                options.UseNpgsql(connectionString));

builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<RuneFlipperContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/logout", async (SignInManager<User> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
})
.RequireAuthorization();

app.UseHttpsRedirection();

app.MapIdentityApi<User>();
app.UseAuthorization();

app.MapControllers();

app.Run();
