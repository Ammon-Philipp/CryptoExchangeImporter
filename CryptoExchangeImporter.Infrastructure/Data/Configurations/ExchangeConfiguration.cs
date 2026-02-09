using CryptoExchangeImporter.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoExchangeImporter.Infrastructure.Data.Configurations;

public class ExchangeConfiguration : IEntityTypeConfiguration<Exchange>
{
    // For Fluent API.
    public void Configure(EntityTypeBuilder<Exchange> builder)
    {
        builder.ToTable("Exchanges");

        builder.HasKey(e => e.Id);

        // Ensure idempotency via unique index on ExchangeId. Enables performance via filtered index.
        builder.HasIndex(e => e.ExchangeId)
               .IsUnique()
               .HasDatabaseName("IX_Exchanges_ExchangeId"); // TODO: Check db implementation.

        // TODO: Check in PR: Length.
        builder.Property(e => e.ExchangeId)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(e => e.CreatedAt)
               .HasColumnType("datetimeoffset")
               .IsRequired();

        // 1:1 relation to AvailableFunds.
        builder.HasOne(e => e.AvailableFunds)
               .WithOne(af => af.Exchange)
               .HasForeignKey<AvailableFunds>(af => af.ExchangeId)
               .OnDelete(DeleteBehavior.Cascade); // Ensure db integrity.

        // 1:1 relation to OrderBook.
        builder.HasOne(e => e.OrderBook)
               .WithOne(ob => ob.Exchange)
               .HasForeignKey<OrderBook>(ob => ob.ExchangeId)
               .OnDelete(DeleteBehavior.Cascade); // Ensure db integrity.
    }
}
