using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using CryptoExchangeImporter.Domain.Entities;

namespace CryptoExchangeImporter.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Exchange> Exchanges => Set<Exchange>();
    public DbSet<AvailableFunds> AvailableFunds => Set<AvailableFunds>();
    public DbSet<OrderBook> OrderBooks => Set<OrderBook>();
    public DbSet<OrderBookEntry> OrderBookEntries => Set<OrderBookEntry>();
    public DbSet<Order> Orders => Set<Order>();
}