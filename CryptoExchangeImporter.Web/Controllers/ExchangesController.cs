using CryptoExchangeImporter.Application.Interfaces;
using CryptoExchangeImporter.Domain.Entities;
using CryptoExchangeImporter.Web.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace CryptoExchangeImporter.Web.Controllers;

public sealed class ExchangesController : Controller
{
    private readonly IExchangeRepository _repo;

    public ExchangesController(IExchangeRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancel)
    {
        var exchanges = await _repo.GetAllAsync(cancel);
        var vm = exchanges.Select(x => new ExchangeListItemViewModel
        {
            Id = x.ExchangeId,
            CreatedAt = x.CreatedAt,
            Crypto = x.AvailableFunds?.Crypto ?? 0,
            Euro = x.AvailableFunds?.Euro ?? 0
        }).ToList();

        // TODO: Correct.
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id, CancellationToken cancel)
    {
        var ex = await _repo.GetByIdAsync(id, cancel);
        if (ex is null) return NotFound();

        var bids = ex.OrderBook?.Bids
            ?.Where(e => e.IsBid)
            .OrderByDescending(e => e.Order.Price)
            .Select(e => OrderRow(e.Order))
            .ToList() ?? new();

        var asks = ex.OrderBook?.Asks
            ?.Where(e => !e.IsBid)
            .OrderBy(e => e.Order.Price)
            .Select(e => OrderRow(e.Order))
            .ToList() ?? new();

        var vm = new ExchangeDetailsViewModel
        {
            Id = ex.ExchangeId,
            CreatedAt = ex.CreatedAt,
            Crypto = ex.AvailableFunds?.Crypto ?? 0,
            Euro = ex.AvailableFunds?.Euro ?? 0,
            Bids = bids,
            Asks = asks
        };

        // TODO: Correct.
        return View(vm);

        // TODO: Change and check parameter type as of Clean Architecture
        static OrderRowViewModel OrderRow(Order order)
        {
            return new OrderRowViewModel()
            {
                // TODO: Implement.
            };
        }

    }
}
