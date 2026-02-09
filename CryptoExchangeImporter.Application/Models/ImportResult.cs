namespace CryptoExchangeImporter.Application.Models;

public sealed class ImportResult
{
    public int ImportedExchanges { get; set; }
    public int SkippedExchanges { get; set; }

    public int ImportedOrders { get; set; }
    public int SkippedDuplicates { get; set; }

    public List<string> Errors { get; } = new();

    public bool Success => Errors.Count == 0;
}
