using Elnes.Data;
using Elnes.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elnes;

public class Startup
{
    public IConfigurationRoot Configuration { get; }

    public Startup()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

        Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton((IConfiguration)Configuration);

        services.AddDbContext<AppDbContext>
            (options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

        services.AddSingleton<ICommandFactory, CommandFactory>();
    }
}
