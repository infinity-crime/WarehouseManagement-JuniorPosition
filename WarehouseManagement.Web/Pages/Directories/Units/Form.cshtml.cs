using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.Web.Pages.Directories.Units
{
    public class FormModel : PageModel
    {
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public FormModel(IUnitOfMeasureService unitOfMeasureService)
        {
            _unitOfMeasureService = unitOfMeasureService;
        }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        public UnitOfMeasureDto? CurrentUnit { get; set; }

        public bool IsNewUnit => Id == Guid.Empty;
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (IsNewUnit)
                return Page();

            var result = await _unitOfMeasureService.GetByIdAsync(id);
            if(result.IsSucces && result.Value != null)
            {
                Name = result.Value.Currency;
                CurrentUnit = result.Value;
                return Page();
            }

            ErrorMessage = result.Error;
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if(IsNewUnit)
                return await CreateNewUnit();

            return await UpdateExistingUnit();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var result = await _unitOfMeasureService.DeleteAsync(Id);
            if(result.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = result.Error;
            return Page();
        }

        public async Task<IActionResult> OnPostArchiveAsync()
        {
            var result = await _unitOfMeasureService.MoveToArchiveAsync(Id);
            if (result.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = result.Error;
            return Page();
        }

        public async Task<IActionResult> OnPostToWorkAsync()
        {
            var result = await _unitOfMeasureService.MoveToWorkAsync(Id);
            if (result.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = result.Error;
            return Page();
        }

        private async Task<IActionResult> CreateNewUnit()
        {
            var result = await _unitOfMeasureService.CreateAsync(Name);
            if (result.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = result.Error;
            return Page();
        }

        private async Task<IActionResult> UpdateExistingUnit()
        {
            var resultUpdate = await _unitOfMeasureService.ChangeNameAsync(Id, Name);
            if (resultUpdate.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = resultUpdate.Error;
            return Page();
        }
    }
}
