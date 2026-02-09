using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoExchangeImporter.Domain.Entities;

public class Exchange
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public AvailableFunds AvailableFunds { get; set; }
    public OrderBook OrderBook { get; set; }
}