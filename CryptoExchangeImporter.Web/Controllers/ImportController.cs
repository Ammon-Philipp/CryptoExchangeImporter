using CryptoExchangeImporter.Application.Services;
using CryptoExchangeImporter.Web.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace CryptoExchangeImporter.Web.Controllers;

public sealed class ImportController : Controller
{
    private readonly ExchangeImportService _importService;

    public ImportController(ExchangeImportService importService) => _importService = importService;

    [HttpGet]
    public IActionResult Index() => View(new UploadResultViewModel());

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancel)
    {
        var vm = new UploadResultViewModel();

        if (file is null || file.Length == 0)
        {
            vm.Errors.Add("File is empty.");
            return View("Index", vm);
        }

        if (!Path.GetExtension(file.FileName).Equals(".json", StringComparison.OrdinalIgnoreCase))
        {
            vm.Errors.Add("Only .json files are allowed.");
            return View("Index", vm);
        }

        await using var stream = file.OpenReadStream();
        var result = await _importService.ImportAsync(stream, file.FileName, cancel);

        vm.ImportedExchanges = result.ImportedExchanges;
        vm.SkippedExchanges = result.SkippedExchanges;
        vm.ImportedOrders = result.ImportedOrders;
        vm.SkippedDuplicateOrders = result.SkippedDuplicateOrders;
        vm.Errors.AddRange(result.Errors.Take(3));

        return View("Index", vm);
    }
}
