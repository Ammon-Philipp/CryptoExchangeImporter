using System.Text.Json;

using CryptoExchangeImporter.Application.DTOs;

namespace CryptoExchangeImporter.Application.Parsing;

public sealed class ExchangeJsonParser : IExchangeJsonParser
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<(ExchangeImportDto? Dto, IReadOnlyList<string> Errors)> ParseAsync(Stream json, CancellationToken cancel)
    {
        var errors = new List<string>();

        ExchangeImportDto? dto;
        try
        {
            dto = await JsonSerializer.DeserializeAsync<ExchangeImportDto>(json, Options, cancel);
        }
        catch (JsonException ex)
        {
            errors.Add($"Invalid JSON: {ex.Message}");
            return (null, errors);
        }

        if (dto is null)
        {
            errors.Add("JSON is empty.");
            return (null, errors);
        }

        // Basic checks.
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            errors.Add("Id is required.");
        }

        if (dto.AvailableFunds is null)
        {
            errors.Add("AvailableFunds is required.");
        }

        if (dto.OrderBook is null)
        {
            errors.Add("OrderBook is required.");
        }
        else
        {
            // Check if Bids / Asks is missing.
            if (dto.OrderBook.Bids is null)
            {
                errors.Add("OrderBook.Bids is required.");
            }

            if (dto.OrderBook.Asks is null)
            {
                errors.Add("OrderBook.Asks is required.");
            }

            // Make sure lists are never null per default.
            dto.OrderBook.Bids ??= new List<OrderBookEntryDto>();
            dto.OrderBook.Asks ??= new List<OrderBookEntryDto>();

            ValidateEntries(dto.OrderBook.Bids, "Bids", errors);
            ValidateEntries(dto.OrderBook.Asks, "Asks", errors);
        }

        return (dto, errors);
    }

    private static void ValidateEntries(IEnumerable<OrderBookEntryDto> dtoEntries, string name, List<string> errors)
    {
        int currIndex = 0;
        foreach (var e in dtoEntries)
        {
            currIndex++;
            var o = e.Order;
            if (o is null) { errors.Add($"{name}[{currIndex}].Order is required."); continue; }

            if (string.IsNullOrWhiteSpace(o.Id))
            {
                errors.Add($"{name}[{currIndex}].Order.Id is required.");
            }

            if (string.IsNullOrWhiteSpace(o.Type))
            {
                errors.Add($"{name}[{currIndex}].Order.Type is required.");
            }

            if (string.IsNullOrWhiteSpace(o.Kind))
            {
                errors.Add($"{name}[{currIndex}].Order.Kind is required.");
            }

            if (o.Amount < 0)
            {
                errors.Add($"{name}[{currIndex}].Order.Amount must be >= 0.");
            }

            if (o.Price < 0)
            {
                errors.Add($"{name}[{currIndex}].Order.Price must be >= 0.");
            }
        }
    }
}
