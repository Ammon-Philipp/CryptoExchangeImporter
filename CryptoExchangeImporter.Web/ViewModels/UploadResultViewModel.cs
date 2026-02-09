namespace CryptoExchangeImporter.Web.ViewModels;

public sealed class UploadResultViewModel
{
    public int ImportedExchanges { get; set; }
    public int SkippedExchanges { get; set; }
    public int ImportedOrders { get; set; }
    public int SkippedDuplicateOrders { get; set; }
    public List<string> Errors { get; } = new();
}
