using CryptoExchangeImporter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoExchangeImporter.Infrastructure.Data.Configurations;

public class OrderBookEntryConfiguration : IEntityTypeConfiguration<OrderBookEntry>
{
    public void Configure(EntityTypeBuilder<OrderBookEntry> builder)
    {
        builder.ToTable("OrderBookEntries");

        builder.HasKey(entry => entry.Id);

        builder.Property(entry => entry.IsBid)
               .IsRequired();

        // 1:1 relation to Order.
        builder.HasOne(entry => entry.Order)
               .WithOne(o => o.OrderBookEntry)
               .HasForeignKey<Order>(o => o.OrderBookEntryId)
               .OnDelete(DeleteBehavior.Cascade); // Ensure db integrity.
    }
}
