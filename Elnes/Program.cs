using Elnes.Factories;
using Elnes.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Elnes;

static partial class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            ArgValidator.StringReadValidation(args);
        }
        catch (ApplicationException validationException)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(validationException.Message);
            Console.ForegroundColor = ConsoleColor.White;
            return 1;
        }

        var serviceProvider = CreateServiceProvider();

        var commandFactory = serviceProvider.GetRequiredService<ICommandFactory>();

        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var command = commandFactory.CreateDataCommand(args);
        await command.ExecuteAsync(token);

        return 0;
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        var startup = new Startup();
        startup.ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}
