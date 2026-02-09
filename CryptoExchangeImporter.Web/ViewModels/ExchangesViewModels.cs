namespace CryptoExchangeImporter.Web.ViewModels;

public sealed class ExchangeListItemViewModel
{
    public string Id { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
    public decimal Crypto { get; set; }
    public decimal Euro { get; set; }
}

public sealed class ExchangeDetailsViewModel
{
    public string Id { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
    public decimal Crypto { get; set; }
    public decimal Euro { get; set; }

    public List<OrderRowViewModel> Bids { get; set; } = new();
    public List<OrderRowViewModel> Asks { get; set; } = new();
}

public sealed class OrderRowViewModel
{
    public string OrderId { get; set; } = "";
    public DateTimeOffset Time { get; set; }
    public string Type { get; set; } = "";
    public string Kind { get; set; } = "";
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
}
