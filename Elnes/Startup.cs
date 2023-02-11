using Elnes.Factories;
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

        // services.AddDbContext<AppDbContext>
        //     (options => options.UseSqlServer(Configuration.GetConnectionString("AuthConnectionString")));

        services.AddSingleton<ICommandFactory, CommandFactory>();
    }
}
