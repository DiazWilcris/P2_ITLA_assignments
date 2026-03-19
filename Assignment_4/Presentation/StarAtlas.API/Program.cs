using Microsoft.EntityFrameworkCore;
using StarAtlas.Application.Services;
using StarAtlas.Infrastructure.Repositories; 
using StarAtlas.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<StarAtlasContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddTransient(typeof(GenericRepository<>));

builder.Services.AddTransient<CelestialBodyRepository>();
builder.Services.AddTransient<BodyTypeRepository>();
builder.Services.AddTransient<ObservationRepository>();
builder.Services.AddTransient<BodyTypeService>();
builder.Services.AddTransient<CelestialBodyService>();
builder.Services.AddTransient<ObservationService>();

builder.Services.AddTransient<UnitOfWork>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();