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
        public UpdateDocumentCommand Command { get; set; } = new()
        {
            Resources = new List<ReceiptResourceItemDto>()
        };

        public bool IsNewReceipt => Id == Guid.Empty;
        public List<SelectListItem> ResourceOptions { get; set; } = new();
        public List<SelectListItem> UnitOptions { get; set; } = new();

        public string? ErrorMessage { get; set; }

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
                            Id = r.Id,
                            ResourceId = r.ResourceId,
                            UnitId = r.UnitId,
                            Amount = r.Amount
                        })
                        .ToList()
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

            ErrorMessage = result.Error;

            await LoadOptions();

            return Page();
        }

        public async Task<IActionResult> OnPostAddResourceAsync()
        {
            if (Command.Resources == null)
                Command.Resources = new List<ReceiptResourceItemDto>();

            Command.Resources.Add(new ReceiptResourceItemDto { Id = Guid.Empty, Amount = 0m });

            await LoadOptions();

            ModelState.Clear();

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveResourceAsync(int? removeIndex, Guid? removeId) // либо по индексу, либо по id
        {
            if (Command.Resources == null)
                Command.Resources = new List<ReceiptResourceItemDto>();

            if (removeIndex.HasValue)
            {
                var idx = removeIndex.Value;
                if (idx >= 0 && idx < Command.Resources.Count)
                    Command.Resources.RemoveAt(idx);
            }
            else if (removeId.HasValue)
            {
                var toRemove = Command.Resources.FirstOrDefault(r => r.Id == removeId.Value);
                if (toRemove != null)
                    Command.Resources.Remove(toRemove);
            }

            await LoadOptions();

            ModelState.Clear();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var result = await _receiptService.DeleteDocumentAsync(Id);
            if (result.IsSucces)
                return RedirectToPage("Receipts");

            ErrorMessage = result.Error;
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
