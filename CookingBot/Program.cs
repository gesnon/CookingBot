using CookingBot.Application.Services;
using CookingBot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<ParserService>();        
        services.AddDbContext<CookingBotContext>(options=>options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CookingBot;Trusted_Connection=True;"));
    })
    .Build();
await host.Services.GetService<ParserService>().Parse();


await host.RunAsync();
