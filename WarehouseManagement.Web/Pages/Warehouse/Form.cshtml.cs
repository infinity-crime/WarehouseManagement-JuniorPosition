using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Application.DTOs.Commands;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.Web.Pages.Warehouse
{
    public class FormModel : PageModel
    {
        private readonly IReceiptService _receiptService;
        private readonly IResourceService _resourceService;
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public FormModel(IReceiptService receiptService, IResourceService resourceService, IUnitOfMeasureService unitOfMeasureService)
        {
            _receiptService = receiptService;
            _resourceService = resourceService;
            _unitOfMeasureService = unitOfMeasureService;
        }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public UpdateDocumentCommand Command { get; set; } = new();

        public bool IsNewReceipt => Id == Guid.Empty;
        public List<SelectListItem> ResourceOptions { get; set; } = new();
        public List<SelectListItem> UnitOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadOptions();

            if(!IsNewReceipt)
            {
                var result = await _receiptService.GetDocumentByIdAsync(Id);
                if(result.IsSucces)
                {
                    Command = new UpdateDocumentCommand
                    {
                        Id = Id,
                        Number = result.Value!.Number,
                        Date = result.Value!.Date,
                        Resources = result.Value!.Resources.Select(r => new ReceiptResourceItemDto
                        {
                            ResourceId = r.ResourceId,
                            UnitId = r.UnitId,
                            Amount = r.Amount
                        }).ToList()
                    };
                }
            }
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            var result = IsNewReceipt ?
                await _receiptService.CreateDocumentAsync(new CreateDocumentCommand
                {
                    Number = Command.Number,
                    Date = Command.Date,
                    Resources = Command.Resources
                })
                : await _receiptService.UpdateDocumentAsync(Command);

            if (result.IsSucces)
                return RedirectToPage("Receipts");

            ModelState.AddModelError(string.Empty, result.Error!);
            await LoadOptions();
            return Page();
        }

        private async Task LoadOptions()
        {
            var resourcesResult = await _resourceService.GetAllResourcesAsync();
            ResourceOptions = resourcesResult.Value!
                .Select(r => new SelectListItem(r.Name, r.Id.ToString()))
                .ToList();

            var unitsResult = await _unitOfMeasureService.GetAllUnitsAsync();
            UnitOptions = unitsResult.Value!
                .Select(u => new SelectListItem(u.Currency, u.Id.ToString()))
                .ToList();
        }
    }
}
