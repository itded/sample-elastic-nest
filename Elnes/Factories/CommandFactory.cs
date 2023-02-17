using Elnes.Commands;
using Elnes.Common;
using Elnes.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elnes.Factories;

public class CommandFactory : ICommandFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public CommandFactory(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public IDataCommand CreateDataCommand(string[] args)
    {
        var createIndex = args[0].ToLower() == Constants.ArgCreateIndexName;
        if (createIndex)
        {
            var nodeUrl = GetConfigNodeUrl();
            return new CreateIndexCommand(nodeUrl);
        }
        
        var createDatabase = args[0].ToLower() == Constants.ArgCreateDbName;
        if (createDatabase)
        {
            var dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            return new CreateDataCommand(dbContext);
        }
        
        var seedDatabase = args[0].ToLower() == Constants.ArgSeedDbName;
        if (seedDatabase)
        {
            var dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            return new SeedFakeDataCommand(dbContext);
        }
        
        var copyRowsToDocsName = args[0].ToLower() == Constants.ArgCopyRowsToDocsName;
        if (copyRowsToDocsName)
        {
            int? teacherId = null;
            if (args[1] != "--all" && int.TryParse(args[1], out var argTeacherId))
            {
                teacherId = argTeacherId;
            }
            
            var nodeUrl = GetConfigNodeUrl();

            var dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            return new CopyDataToElasticCommand(dbContext, nodeUrl, teacherId);
        }

        throw new InvalidOperationException();
    }

    private Uri GetConfigNodeUrl()
    {
        var nodeUrl = _configuration.GetSection("Elastic")["NodeUrl"];

        if (string.IsNullOrWhiteSpace(nodeUrl))
        {
            throw new ArgumentNullException(nameof(nodeUrl));
        }

        return new Uri(nodeUrl);
    }
}
