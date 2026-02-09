using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoExchangeImporter.Domain.Entities;

public class Exchange
{
    public int Id { get; set; }
    public string ExchangeId { get; set; }
    public DateTime ImportedAt { get; set; }

    public AvailableFunds AvailableFunds { get; set; }
    public OrderBook OrderBook { get; set; }
}