using Elnes.Commands;

namespace Elnes.Factories;

public interface ICommandFactory
{
    IDataCommand CreateDataCommand(string[] args);
}