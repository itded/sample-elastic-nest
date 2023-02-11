using Elnes.Commands;
using Elnes.Common;
using Microsoft.Extensions.Configuration;

namespace Elnes.Factories;

public class CommandFactory : ICommandFactory
{
    private readonly IConfiguration _configuration;

    public CommandFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDataCommand CreateDataCommand(string[] args)
    {
        var createIndex = args[0].ToLower() == Constants.ArgCreateIndexName;
        if (createIndex)
        {
            var nodeUrl = _configuration.GetSection("Elastic")["NodeUrl"];

            if (string.IsNullOrWhiteSpace(nodeUrl))
            {
                throw new ArgumentNullException(nameof(nodeUrl));
            }
            
            return new CreateIndexCommand(new Uri(nodeUrl));
        }

        throw new InvalidOperationException();
    }
}
