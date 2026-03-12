using Microsoft.EntityFrameworkCore;
using StarAtlas.Persistence.Context;
using StarAtlas.Infrastructure.Repositories; 

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

builder.Services.AddTransient<UnitOfWork>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();