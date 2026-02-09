using System.Text.Json.Serialization;

namespace CryptoExchangeImporter.Application.DTOs;

public sealed class ExchangeImportDto
{
    [JsonPropertyName("Id")]
    public string? Id { get; set; }

    [JsonPropertyName("AvailableFunds")]
    public AvailableFundsDto? AvailableFunds { get; set; }

    [JsonPropertyName("OrderBook")]
    public OrderBookDto? OrderBook { get; set; }
}

public sealed class AvailableFundsDto
{
    [JsonPropertyName("Crypto")]
    public decimal Crypto { get; set; }

    [JsonPropertyName("Euro")]
    public decimal Euro { get; set; }
}

public sealed class OrderBookDto
{
    [JsonPropertyName("Bids")]
    public List<OrderBookEntryDto>? Bids { get; set; }

    [JsonPropertyName("Asks")]
    public List<OrderBookEntryDto>? Asks { get; set; }
}

public sealed class OrderBookEntryDto
{
    [JsonPropertyName("Order")]
    public OrderDto? Order { get; set; }
}

public sealed class OrderDto
{
    [JsonPropertyName("Id")]
    public string? Id { get; set; }

    [JsonPropertyName("Time")]
    public DateTimeOffset Time { get; set; }

    [JsonPropertyName("Type")]
    public string? Type { get; set; }

    [JsonPropertyName("Kind")]
    public string? Kind { get; set; }

    [JsonPropertyName("Amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("Price")]
    public decimal Price { get; set; }
}
