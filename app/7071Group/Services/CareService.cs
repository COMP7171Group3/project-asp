using _7071Group.Data;
using _7071Group.Models;
using Microsoft.EntityFrameworkCore;

public class CareService
{
    private readonly CareDbContext _careDbContext;

    public CareService(CareDbContext careDbContext)
    {
        _careDbContext = careDbContext;
    }

    public async Task AddClientAsync(Client newClient)
    {
        // Start local transaction
        using var tx = await _careDbContext.Database.BeginTransactionAsync();
        try
        {
            _careDbContext.Clients.Add(newClient);
            await _careDbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
