using BarberApi.Extensions;
using DotNetEnv;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabase();
builder.Services.AddCustomServices();
builder.Services.AddAuthentication();
builder.Services.AddControllers();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();