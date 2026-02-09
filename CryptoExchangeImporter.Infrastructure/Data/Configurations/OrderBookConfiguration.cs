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
               .HasMaxLength(50) // TODO: Check in PR.
               .IsRequired();

        // 1:N relation to OrderBookEntry.
        builder.HasMany<OrderBookEntry>()
               .WithOne(obe => obe.OrderBook)
               .HasForeignKey(obe => obe.OrderBookId)
               .OnDelete(DeleteBehavior.Cascade);   // Ensure db integrity.

        // Ignore Bids and Asks as there is only a relationship between OrderBook and OrderBookEntry.
        builder.Ignore(ob => ob.Bids);
        builder.Ignore(ob => ob.Asks);
    }
}
