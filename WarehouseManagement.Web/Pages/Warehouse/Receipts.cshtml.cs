using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.Web.Pages.Warehouse
{
    public class ReceiptsModel : PageModel
    {
        private readonly IReceiptService _receiptService;
        private readonly IResourceService _resourceService;
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public ReceiptsModel(IReceiptService receiptService, IResourceService resourceService, IUnitOfMeasureService unitOfMeasureService)
        {
            _receiptService = receiptService;
            _resourceService = resourceService;
            _unitOfMeasureService = unitOfMeasureService;
        }

        [BindProperty(SupportsGet = true)]
        public ReceiptRecordFilter Filter { get; set; } = new();

        public List<ReceiptRecordDto> ReceiptRecords { get; set; } = new();

        public List<SelectListItem> DocumentNumberOptions { get; set; } = new();
        public List<SelectListItem> ResourceOptions { get; set; } = new();
        public List<SelectListItem> UnitOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            if(Filter.StartDate.HasValue)
                Filter.StartDate = Filter.StartDate.Value.ToUniversalTime();

            if(Filter.EndDate.HasValue)
                Filter.EndDate = Filter.EndDate.Value.ToUniversalTime();

            await LoadFilterOptions();

            var result = await _receiptService.GetReceiptRecordsAsync(Filter);
            if (result.IsSucces)
                ReceiptRecords = result.Value!.ToList();
        }

        private async Task LoadFilterOptions()
        {
            // загружаем номера документов
            var receiptNumbers = (await _receiptService.GetReceiptRecordsAsync(new ReceiptRecordFilter())).Value!
                .Select(r => r.DocumentNumber)
                .Distinct()
                .OrderBy(n => n);

            DocumentNumberOptions = receiptNumbers
                .Select(n => new SelectListItem(n, n))
                .ToList();

            // загружаем ресурсы, которые активны и находятся в архиве
            var resourcesResult = await _resourceService.GetAllResourcesAsync();
            ResourceOptions = resourcesResult.Value!
                    .Select(r => new SelectListItem(r.Name, r.Id.ToString()))
                    .ToList();

            // загружаем единицы измерения
            var unitsResult = await _unitOfMeasureService.GetAllUnitsAsync();
            UnitOptions = unitsResult.Value!
                .Select(u => new SelectListItem(u.Currency, u.Id.ToString()))
                .ToList();
        }
    }
}
