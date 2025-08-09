using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Application.Services;
using WarehouseManagement.Domain.Interfaces;

namespace WarehouseManagement.Web.Pages.Directories.Units
{
    public class ArchiveModel : PageModel
    {
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public ArchiveModel(IUnitOfMeasureService unitOfMeasureService)
        {
            _unitOfMeasureService = unitOfMeasureService;
        }

        public List<UnitOfMeasureDto> Units { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var result = await _unitOfMeasureService.GetAllArchiveUnitsAsync();
            Units = result.Value!.ToList();

            return Page();
        }
    }
}
