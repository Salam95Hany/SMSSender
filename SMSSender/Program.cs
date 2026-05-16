using Microsoft.AspNetCore.Http.Features;
using SMSSender.DI;
using SMSSender.Hubs;
using SMSSender.Messaging;
using SMSSender.Services.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppPaths>(options =>
{
    var env = builder.Environment;
    options.WebRootPath = env.WebRootPath;
});

builder.Services.AddDependencies(builder.Configuration);
builder.Services.Bootstrap();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = 10000;
    options.MultipartBodyLengthLimit = 200_000_000;
    options.ValueLengthLimit = 16384;
    options.MemoryBufferThreshold = 81920;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(DependencyInjection.GetCorsPolicyName());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.UseStaticFiles();
app.Run();
