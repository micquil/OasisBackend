using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure PostgreSQL Database for EF Core
builder.Services.AddDbContext<OasisDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configure PostgreSQL for Dapper (Singleton)
builder.Services.AddSingleton<DapperContext>(provider => new DapperContext(connectionString));

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:5173", "https://a4v0.netlify.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
