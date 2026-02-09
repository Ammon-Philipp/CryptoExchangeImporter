using CryptoExchangeImporter.Application.DTOs;

namespace CryptoExchangeImporter.Application.Parsing;

public interface IExchangeJsonParser
{
    Task<(ExchangeImportDto? Dto, IReadOnlyList<string> Errors)> ParseAsync(Stream json, CancellationToken cancel);
}
