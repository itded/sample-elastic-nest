namespace Elnes.Commands;

public interface IDataCommand
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}