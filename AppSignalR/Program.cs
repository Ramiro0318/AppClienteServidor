using AppSignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
var app = builder.Build();

app.MapHub<ChatHub>("/hubs/chat");

app.MapGet("/", () => "Hello World!");

app.Run();
