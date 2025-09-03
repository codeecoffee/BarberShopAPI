using BarberApi.Data;
using BarberApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using DotNetEnv;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddDatabase();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();