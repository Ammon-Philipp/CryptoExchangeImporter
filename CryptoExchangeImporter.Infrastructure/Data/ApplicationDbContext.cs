using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using CryptoExchangeImporter.Domain.Entities;

namespace CryptoExchangeImporter.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    // TODO: Register in Program.cs.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Exchange> Exchanges => Set<Exchange>();
    public DbSet<AvailableFunds> AvailableFunds => Set<AvailableFunds>();
    public DbSet<OrderBook> OrderBooks => Set<OrderBook>();
    public DbSet<OrderBookEntry> OrderBookEntries => Set<OrderBookEntry>();
    public DbSet<Order> Orders => Set<Order>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TODO: Use Fluent API to configure the entity relationships.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
}