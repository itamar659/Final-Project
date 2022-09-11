using Microsoft.EntityFrameworkCore;
using Server;
using Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ServerContext")
        ?? throw new InvalidOperationException("Connection string 'ServerContext' not found.")));

builder.Services
    .AddSignalR()
    .AddAzureSignalR(
    "Endpoint=https://chat-test-chsarp-proj.service.signalr.net;AccessKey=w3A8h3bz4Q+1TcUrn8TFklTrhICn9jbEXunMUtOFHaA=;Version=1.0;"
);

builder.AddCustomServices();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection(); // use https redirect in production
}

app.UseRouting();
app.UseFileServer();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<RoomHub>("/roomhub");
});

app.MapControllers();

app.Run();
