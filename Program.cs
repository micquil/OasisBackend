using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure SQL Server for Dapper (Singleton)
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

// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;

// var builder = WebApplication.CreateBuilder(args);

// // Add services
// builder.Services.AddControllers();
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//         policy.AllowAnyOrigin()
//               .AllowAnyHeader()
//               .AllowAnyMethod());
// });

// var app = builder.Build();

// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error");
//     app.UseHsts();
// }

// // Serve React Static Files (from "dist" or "build" folder)
// app.UseStaticFiles();

// app.UseRouting();
// app.UseCors("AllowAll");

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
// });

// // **Serve React Frontend**
// app.Use(async (context, next) =>
// {
//     if (context.Request.Path.StartsWithSegments("/api"))
//     {
//         await next();
//     }
//     else
//     {
//         context.Response.ContentType = "text/html";
//         await context.Response.SendFileAsync("ClientApp/build/index.html");
//     }
// });

// app.Run();
