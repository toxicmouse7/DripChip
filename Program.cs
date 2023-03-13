using System.Text.Json.Serialization;
using DripChip.Authentication;
using DripChip.Models;
using DripChip.Models.DataTransferObjects;
using DripChip.Models.DataTransferObjects.Accounts;
using DripChip.Models.DataTransferObjects.Animals;
using DripChip.Models.DataTransferObjects.AnimalTypes;
using DripChip.Models.DataTransferObjects.Locations;
using DripChip.Models.Entities;
using DripChip.Models.FilterData;
using DripChip.Models.Mappers;
using DripChip.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
    });


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connectionString));

builder.Services
    .AddTransient<AccountsRepository>()
    .AddTransient<IRepository<User>, AccountsRepository>(x => x.GetRequiredService<AccountsRepository>())
    .AddTransient<IFilterable<User, UsersFilterData>, AccountsRepository>(x =>
        x.GetRequiredService<AccountsRepository>());

builder.Services
    .AddTransient<AnimalsRepository>()
    .AddTransient<IRepository<Animal>, AnimalsRepository>(x => x.GetRequiredService<AnimalsRepository>())
    .AddTransient<IFilterable<Animal, AnimalsFilterData>, AnimalsRepository>(x =>
        x.GetRequiredService<AnimalsRepository>());

builder.Services.AddTransient<IRepository<AnimalType>, AnimalTypesRepository>();
builder.Services.AddTransient<IRepository<Location>, LocationsRepository>();

builder.Services
    .AddTransient<AnimalMapper>()
    .AddTransient<IMapper<Animal, AnimalCreationDto, AnimalUpdateDto, AnimalResponseDto>, AnimalMapper>(
        x => x.GetRequiredService<AnimalMapper>()
    );

builder.Services
    .AddTransient<UserMapper>()
    .AddTransient<IMapper<User, UserCreationDto, UserUpdateDto, UserRepsonseDto>, UserMapper>(
        x => x.GetRequiredService<UserMapper>()
    );

builder.Services
    .AddTransient<AnimalTypeMapper>()
    .AddTransient<IMapper<AnimalType, AnimalTypeCreationDto, AnimalTypeCreationDto, AnimalTypeResponseDto>,
        AnimalTypeMapper>(
        x => x.GetRequiredService<AnimalTypeMapper>()
    );

builder.Services
    .AddTransient<LocationMapper>()
    .AddTransient<IMapper<Location, LocationCreationDto, LocationCreationDto, LocationResponseDto>, LocationMapper>(
        x => x.GetRequiredService<LocationMapper>()
    );

builder.Services
    .AddAuthentication(options => options.DefaultScheme = nameof(DripChipAuthSchemeOptions))
    .AddScheme<DripChipAuthSchemeOptions, DripChipAuthHandler>(nameof(DripChipAuthSchemeOptions), _ => { });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();