using Elnes.Elastic.Indices;
using Elnes.Exceptions;
using Nest;

namespace Elnes.Commands;

/// <summary>
/// Creates a test index if it doesn't exist 
/// </summary>
public class CreateIndexCommand : IDataCommand
{
    private readonly Uri _esNodeUri;

    public CreateIndexCommand(Uri esNodeUri)
    {
        _esNodeUri = esNodeUri;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var esConfig = new ConnectionSettings(_esNodeUri);
        var esClient = new ElasticClient(esConfig);
 
        var settings = new IndexSettings { NumberOfReplicas = 1, NumberOfShards = 1 };
 
        var indexConfig = new IndexState
        {
            Settings = settings
        };

        var result = await esClient.Indices.ExistsAsync(Common.Constants.ElasticTeacherIndexName, null, cancellationToken);
        if (!result.Exists)
        {
            await esClient.Indices.CreateAsync(Common.Constants.ElasticTeacherIndexName, c => c
                .InitializeUsing(indexConfig)
                .Map(m => m.AutoMap<TeacherIndex>()), cancellationToken);
        }
        else
        {
            throw new IndexExistsException(Common.Constants.ElasticTeacherIndexName);
        }
    }
}