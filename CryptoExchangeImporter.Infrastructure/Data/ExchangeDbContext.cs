using System.Reflection;
using CryptoExchangeImporter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoExchangeImporter.Infrastructure.Data;

public class ExchangeDbContext : DbContext
{
    public ExchangeDbContext(DbContextOptions<ExchangeDbContext> options)
        : base(options) { }

    public DbSet<Exchange> Exchanges => Set<Exchange>();
    public DbSet<AvailableFunds> AvailableFunds => Set<AvailableFunds>();
    public DbSet<OrderBook> OrderBooks => Set<OrderBook>();
    public DbSet<OrderBookEntry> OrderBookEntries => Set<OrderBookEntry>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
