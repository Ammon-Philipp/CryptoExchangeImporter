using CryptoExchangeImporter.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoExchangeImporter.Infrastructure.Data.Configurations;

public class OrderBookConfiguration : IEntityTypeConfiguration<OrderBook>
{
    public void Configure(EntityTypeBuilder<OrderBook> builder)
    {
        builder.ToTable("OrderBooks");

        builder.HasKey(ob => ob.Id);

        builder.Property(ob => ob.ExchangeId)
            .HasMaxLength(50)
            .IsRequired();

        // 1:N relation to Bids.
        builder.HasMany(ob => ob.Bids)
            .WithOne(obe => obe.OrderBook)
            .HasForeignKey(obe => obe.OrderBookId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1:N relation to Asks.
        builder.HasMany(ob => ob.Asks)
            .WithOne(obe => obe.OrderBook)
            .HasForeignKey(obe => obe.OrderBookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}