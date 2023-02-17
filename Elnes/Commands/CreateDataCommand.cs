using Elnes.Data;
using Microsoft.EntityFrameworkCore;

namespace Elnes.Commands;

public class CreateDataCommand : IDataCommand
{
    private readonly AppDbContext _context;

    public CreateDataCommand(AppDbContext context)
    {
        _context = context;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await _context.Database.MigrateAsync(cancellationToken);
    }
}