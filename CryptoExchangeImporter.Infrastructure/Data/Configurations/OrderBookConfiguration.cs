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
               .IsRequired();

        // EF uses backing field here as Bids and Asks are only projections, no tables.
        builder.HasMany<OrderBookEntry>("_entries")
               .WithOne(e => e.OrderBook)
               .HasForeignKey(e => e.OrderBookId)
               .OnDelete(DeleteBehavior.Cascade);   // Ensure db integrity.

        // Ignore Bids and Asks as there is only a relationship between OrderBook and OrderBookEntry.
        builder.Ignore(ob => ob.Bids);
        builder.Ignore(ob => ob.Asks);
    }
}
