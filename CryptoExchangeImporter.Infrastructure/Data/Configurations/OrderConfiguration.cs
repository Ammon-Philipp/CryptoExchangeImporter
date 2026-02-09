using CryptoExchangeImporter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoExchangeImporter.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        // TODO: Check in PR: Compare Unique index with unique constraint.
        // Ensure idempotency via unique index on OrderId. Enables performance via filtered index.
        builder.HasIndex(o => o.OrderId)
            .IsUnique()
            .HasDatabaseName("IX_Orders_OrderId");  // TODO: Check db implementation.

        builder.Property(o => o.OrderId)
            .IsRequired();

        // TODO: Implement DateTimeOffset usage in service that handles import.
        builder.Property(o => o.Time)
            .HasColumnType("datetimeoffset")
            .IsRequired();

        builder.Property(o => o.Type)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(o => o.Kind)
            .HasConversion<string>()
            .HasMaxLength(50)                   // For longer names, e.g. 'Time-Weighted Average Price'
            .IsRequired();

        builder.Property(o => o.Amount)
            .HasColumnType("decimal(18,8)")     // Reflects AvailableFunds decimal precision. => Only BTC.
            .IsRequired();

        builder.Property(o => o.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}