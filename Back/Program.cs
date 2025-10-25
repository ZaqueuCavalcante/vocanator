var builder = WebApplication.CreateBuilder(args);

builder.AddSettingsConfigs();
builder.AddServicesConfigs();

builder.AddHttpConfigs();

builder.AddDapperConfigs();
builder.AddEfCoreConfigs();

builder.AddDocsConfigs();
builder.AddCorsConfigs();

var app = builder.Build();


var ctx = app.Services.CreateScope().ServiceProvider.GetRequiredService<VocanatorDbContext>();
ctx.Database.EnsureDeleted();
ctx.Database.EnsureCreated();


app.UseCors();

app.UseRouting();
app.UseExceptions();

app.UseDocs();

app.UseControllers();

app.Run();

public partial class Program { }
