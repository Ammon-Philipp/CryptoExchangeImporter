using System.ComponentModel;

using CryptoExchangeImporter.Application.DTOs;
using CryptoExchangeImporter.Application.Interfaces;
using CryptoExchangeImporter.Application.Models;
using CryptoExchangeImporter.Application.Parsing;
using CryptoExchangeImporter.Domain.Entities;
using CryptoExchangeImporter.Domain.Enums;

using Microsoft.Extensions.Logging;

namespace CryptoExchangeImporter.Application.Services;

public sealed class ExchangeImportService
{
    private readonly IExchangeJsonParser _parser;
    private readonly IExchangeRepository _repo;
    private readonly ILogger<ExchangeImportService> _log;

    public ExchangeImportService(IExchangeJsonParser parser, IExchangeRepository repo, ILogger<ExchangeImportService> log)
    {
        _parser = parser;
        _repo = repo;
        _log = log;
    }

    public async Task<ImportResult> ImportAsync(Stream json, string? fileName, CancellationToken cancel)
    {
        var result = new ImportResult();

        _log.LogInformation("Begin import. File={FileName}", fileName);

        var (dto, parseErrors) = await _parser.ParseAsync(json, cancel);
        if (dto is null)
        {
            result.Errors.AddRange(parseErrors);
            return result;
        }

        if (parseErrors.Count > 0)
        {
            // Validation could fail.
            result.Errors.AddRange(parseErrors);
            return result;
        }

        var exchangeId = dto.Id!;
        if (await _repo.ExistsAsync(exchangeId, cancel))
        {
            result.SkippedExchanges++;
            _log.LogInformation("Import skipped. Exchange already known: ExchangeId={ExchangeId}", exchangeId);
            return result;
        }

        var incomingOrderIds = GetIncomingOrderIds(dto)
                               .Distinct()
                               .ToList();

        var existingOrderIds = await _repo.GetExistingOrderIdsAsync(incomingOrderIds, cancel);

        // Map DTO -> Whole Domain Aggregate.
        var (exchange, skippedDuplicates) = MapToDomain(dto, existingOrderIds);
        result.SkippedDuplicateOrders += skippedDuplicates;


        try
        {
            var (importedExchanges, importedOrders, skippedDuplicateOrders) = await _repo.AddAsync(exchange, cancel);
            result.ImportedExchanges += importedExchanges;
            result.ImportedOrders += importedOrders;
            result.SkippedDuplicateOrders += skippedDuplicateOrders;
        }
        catch (Exception ex)
        {
            result.Errors.Add($"Import failed: {ex.Message}");
            _log.LogError(ex, "Import failed. ExchangeId={ExchangeId}", exchangeId);
            return result;
        }

        _log.LogInformation(
            "Import completed. ExchangeId={ExchangeId} ImportedOrders={ImportedOrders} SkippedDupOrders={SkippedDupOrders}",
            exchangeId, result.ImportedOrders, result.SkippedDuplicateOrders);

        return result;
    }

    private static IEnumerable<string> GetIncomingOrderIds(ExchangeImportDto dto)
    {
        var bids = dto.OrderBook?.Bids;
        if (bids != null)
        {
            foreach (var e in bids)
            {
                var id = e?.Order?.Id;
                if (!string.IsNullOrWhiteSpace(id))
                {
                    yield return id.Trim();
                }
            }
        }

        var asks = dto.OrderBook?.Asks;
        if (asks != null)
        {
            foreach (var e in asks)
            {
                var id = e?.Order?.Id;
                if (!string.IsNullOrWhiteSpace(id))
                {
                    yield return id.Trim();
                }
            }
        }
    }


    private static (Exchange Exchange, int SkippedDuplicates) MapToDomain(ExchangeImportDto dto, IReadOnlySet<string> existingOrderIds)
    {
        var exchange = new Exchange(dto.Id!, DateTimeOffset.UtcNow);

        var funds = new AvailableFunds(
            crypto: dto.AvailableFunds.Crypto,
            euro: dto.AvailableFunds.Euro
        );

        exchange.SetAvailableFunds(funds);

        var orderBook = new OrderBook();

        // Necessary as orderIds can also be duplicates within incoming data itself.
        var alreadySeenIncomingOrderIds = new HashSet<string>();

        // TODO: Handling overflow necessary for really big incoming data?
        int skipped = 0;

        foreach (var bidDto in dto.OrderBook.Bids!)
        {
            var orderId = bidDto?.Order?.Id?.Trim();
            if (string.IsNullOrWhiteSpace(orderId)) { continue; }

            if (existingOrderIds.Contains(orderId) || !alreadySeenIncomingOrderIds.Add(orderId))
            {
                skipped++;
                continue;
            }

            orderBook.AddBid(MapOrder(bidDto));
        }

        foreach (var askDto in dto.OrderBook.Asks!)
        {
            var orderId = askDto?.Order?.Id?.Trim();
            if (string.IsNullOrWhiteSpace(orderId)) { continue; }

            if (existingOrderIds.Contains(orderId) || !alreadySeenIncomingOrderIds.Add(orderId))
            {
                skipped++;
                continue;
            }

            orderBook.AddAsk(MapOrder(askDto));
        }

        exchange.SetOrderBook(orderBook);

        return (exchange, skipped);
    }


    private static Order MapOrder(OrderBookEntryDto orderBookEntryDto)
    {
        var orderDto = orderBookEntryDto.Order;

        return new Order(orderId: orderDto.Id!,
                         time: orderDto.Time,
                         type: MapOrderType(orderDto.Type),
                         kind: MapOrderKind(orderDto.Kind),
                         amount: orderDto.Amount,
                         price: orderDto.Price
        );
    }

    private static OrderType MapOrderType(string? value)
    {
        // Already validatied via ExchangeImporter.ValidateEntries.
        var trimmed = value?.Trim();

        return trimmed.ToLowerInvariant() switch
        {
            "buy" => OrderType.Buy,
            "sell" => OrderType.Sell,
            _ => OrderType.Unknown
        };
    }

    private static OrderKind MapOrderKind(string? value)
    {
        // Already validatied via ExchangeImporter.ValidateEntries.
        var trimmed = value?.Trim();

        return trimmed.ToLowerInvariant() switch
        {
            "limit" => OrderKind.Limit,
            "market" => OrderKind.Market,
            "stop" => OrderKind.Stop,
            _ => OrderKind.Unknown
        };
    }
}
