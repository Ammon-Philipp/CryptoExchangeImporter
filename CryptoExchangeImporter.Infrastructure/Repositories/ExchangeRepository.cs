using CryptoExchangeImporter.Application.Interfaces;
using CryptoExchangeImporter.Domain;
using CryptoExchangeImporter.Domain.Entities;
using CryptoExchangeImporter.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace CryptoExchangeImporter.Infrastructure.Repositories;

public sealed class ExchangeRepository : IExchangeRepository
{
    private readonly ExchangeDbContext _db;

    public ExchangeRepository(ExchangeDbContext db) => _db = db;

    public Task<bool> ExistsAsync(string exchangeId, CancellationToken cancel)
        => _db.Exchanges.AsNoTracking().AnyAsync(x => x.ExchangeId == exchangeId, cancel);

    public async Task<AddExchangeResult> AddAsync(Exchange exchange, CancellationToken cancel)
    {
        // Import whole exchange or nnothin at all.
        await using var transactionAsync = await _db.Database.BeginTransactionAsync(cancel);

        _db.Exchanges.Add(exchange);

        try
        {
            var changes = await _db.SaveChangesAsync(cancel);
            await transactionAsync.CommitAsync(cancel);

            return new AddExchangeResult
            {
                ImportedExchanges = 1,
                ImportedOrders = CountOrders(exchange),
                SkippedDuplicateOrders = 0
            };
        }
        catch (DbUpdateException)
        {
            // TODO: Retry without duplicates.
            await transactionAsync.RollbackAsync(cancel);
            throw;
        }
    }

    public async Task<IReadOnlyList<Exchange>> GetAllAsync(CancellationToken cancel)
        => await _db.Exchanges
            .AsNoTracking()
            .Include(x => x.AvailableFunds)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancel);

    public Task<Exchange?> GetByIdAsync(string id, CancellationToken cancel)
        => _db.Exchanges
            .AsNoTracking()
            .Include(x => x.AvailableFunds)
            .Include(x => x.OrderBook)
                .ThenInclude(ob => ob.Bids)
                .ThenInclude(e => e.Order)
            .Include(x => x.OrderBook)
                .ThenInclude(ob => ob.Asks)
                .ThenInclude(e => e.Order)
            .FirstOrDefaultAsync(x => x.ExchangeId == id, cancel);

    private static int CountOrders(Exchange exchange)
        => (exchange.OrderBook?.Bids?.Count ?? 0) + (exchange.OrderBook?.Asks?.Count ?? 0);
}
