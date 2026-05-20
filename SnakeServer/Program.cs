using SnakeServer.Hubs;
using SnakeServer.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();
builder.Services.AddScoped<SalasService>();

var app = builder.Build();

app.MapHub<GameHub>("/Vivora");
app.UseFileServer();    //Es como UseStaticFiles + un ruteo automatico a index.


app.Run();
