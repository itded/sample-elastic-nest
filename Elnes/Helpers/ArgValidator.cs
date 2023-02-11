using System.Diagnostics;
using Elnes.Common;

namespace Elnes.Helpers;

public static class ArgValidator
{
    public static void StringReadValidation(string[] args)
    {
        var invalidArgLen = args.Length == 0;
        if (invalidArgLen)
        {
            ThrowUnknownCommandException();
        }

        var createIndex = args[0].ToLower() == Constants.ArgCreateIndexName;
        var createDb = args[0].ToLower() == Constants.ArgCreateDbName;
        var seedDb = args[0].ToLower() == Constants.ArgSeedDbName;
        var copyRows = args[0].ToLower() == Constants.ArgCopyRowsToDocsName;
        var invalidMode = !createIndex && !createDb && !seedDb && !copyRows;
        if (invalidMode)
        {
            ThrowUnknownCommandException();
        }

        if (copyRows && (args.Length == 1 || args[1] != "--all" || !int.TryParse(args[1], out _)))
        {
            ThrowUnknownCommandException();
        }
    }

    private static void ThrowUnknownCommandException()
    {
        var fileName = Process.GetCurrentProcess().MainModule?.FileName ?? "Elnes.exe";
        throw new ApplicationException($"Unknown command.\nPlease use one of the next commands:\n" +
                                       $"{fileName} {Constants.ArgCreateIndexName}\n" +
                                       $"{fileName} {Constants.ArgCreateDbName}\n" +
                                       $"{fileName} {Constants.ArgSeedDbName}\n" +
                                       $"{fileName} {Constants.ArgCopyRowsToDocsName} --all|[id]");
    }
}
