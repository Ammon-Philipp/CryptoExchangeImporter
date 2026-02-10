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

        // Map DTO -> Whole Domain Aggregate.
        // TODO: Check in PR: .
        var exchange = MapToDomain(dto);

        try
        {
            var save = await _repo.AddAsync(exchange, cancel);
            result.ImportedExchanges += save.ImportedExchanges;
            result.ImportedOrders += save.ImportedOrders;
            result.SkippedDuplicateOrders += save.SkippedDuplicateOrders;
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

    private static Exchange MapToDomain(ExchangeImportDto dto)
    {
        // Define import timestamp as now => Set createdAt.
        var exchange = new Exchange(dto.Id!, DateTimeOffset.UtcNow);

        var funds = new AvailableFunds(
            crypto: dto.AvailableFunds.Crypto,
            euro: dto.AvailableFunds.Euro
        );

        exchange.SetAvailableFunds(funds);
        var orderBook = new OrderBook();

        // TODO: Finish up entity rebuild.
        foreach (var bidDto in dto.OrderBook.Bids!)
        {
            orderBook.AddBid(MapOrder(bidDto));
        }

        foreach (var askDto in dto.OrderBook.Asks!)
        {
            orderBook.AddAsk(MapOrder(askDto));
        }

        exchange.SetOrderBook(orderBook);

        return exchange;
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
