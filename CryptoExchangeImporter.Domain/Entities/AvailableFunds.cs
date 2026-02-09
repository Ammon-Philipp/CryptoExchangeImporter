using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoExchangeImporter.Domain.Entities;

public class AvailableFunds
{
    public int Id { get; set; }
    public decimal Crypto { get; set; }
    public decimal Euro { get; set; }

    // FK
    public int ExchangeId { get; set; }
    public Exchange Exchange { get; set; }
}
