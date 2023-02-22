using DripChip.Authentication;
using DripChip.Models;
using DripChip.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connectionString));
builder.Services.AddTransient<IAccountsService, AccountsService>();
builder.Services.AddTransient<IAnimalsService, AnimalsService>();
builder.Services.AddTransient<IAnimalTypesService, AnimalTypesService>();
builder.Services.AddTransient<ILocationsService, LocationsService>();
builder.Services
    .AddAuthentication(
        options => options.DefaultScheme = nameof(DripChipAuthSchemeOptions))
    .AddScheme<DripChipAuthSchemeOptions, DripChipAuthHandler>(
        nameof(DripChipAuthSchemeOptions), _ => { });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();