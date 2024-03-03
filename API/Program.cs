// using API.Repositories;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.

// builder.Services.AddControllers();
// builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
// builder.Services.AddSingleton<IUserRepository, UserRepository>();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// builder.Services.AddCors(p => p.AddPolicy("corsapp",builder =>
// {
//     builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
// }));


// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
// app.UseCors("corsapp");

// app.UseAuthorization();

// app.MapControllers();

// app.Run();

using API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}

app.UseCors("corsapp");

app.UseHttpsRedirection();

app.UseAuthorization();

// Enable session
app.UseSession();

app.MapControllers();

app.Run();