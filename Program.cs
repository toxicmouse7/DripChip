using DripChip.Authentication;
using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.FilterData;
using DripChip.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connectionString));

builder.Services.AddTransient<IRepository<User>, AccountsRepository>();
builder.Services.AddTransient<IFilterable<User, UsersFilterData>, AccountsRepository>();
builder.Services.AddTransient<IRepository<Animal>, AnimalsRepository>();
builder.Services.AddTransient<IFilterable<Animal, AnimalsFilterData>, AnimalsRepository>();
builder.Services.AddTransient<IRepository<AnimalType>, AnimalTypesRepository>();
builder.Services.AddTransient<IRepository<Location>, LocationsRepository>();

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