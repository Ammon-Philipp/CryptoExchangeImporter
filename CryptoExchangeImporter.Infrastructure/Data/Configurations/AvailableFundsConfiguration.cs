using CryptoExchangeImporter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoExchangeImporter.Infrastructure.Data.Configurations;

public class AvailableFundsConfiguration : IEntityTypeConfiguration<AvailableFunds>
{
    // For Fluent API.
    public void Configure(EntityTypeBuilder<AvailableFunds> builder)
    {
        builder.ToTable("AvailableFunds");

        builder.HasKey(af => af.Id);

        builder.Property(af => af.Crypto)
               .HasColumnType("decimal(18,8)") // Direct limitation on BitCoin, as other Crypto currencies may have precision with 6 - 18 decimal places.
               .IsRequired();

        builder.Property(af => af.Euro)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(af => af.ExchangeId)
               .IsRequired();
    }
}
