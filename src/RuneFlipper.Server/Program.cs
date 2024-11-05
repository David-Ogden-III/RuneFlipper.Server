using DataAccessLayer;
using MailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("RuneFlipperDb");

builder.Services.AddDbContext<RuneFlipperContext>(
    options =>
        options.UseNpgsql(
            connectionString,
            x => x.MigrationsAssembly("DataAccessLayer")));

builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<RuneFlipperContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddTransient<IEmailSender, MailSender>();
builder.Services.Configure<MailSenderOptions>(builder.Configuration);

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
