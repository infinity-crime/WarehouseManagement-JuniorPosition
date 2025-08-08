using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.Web.Pages.Directories.Units
{
    public class MainModel : PageModel
    {
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public MainModel(IUnitOfMeasureService unitOfMeasureService)
        {
            _unitOfMeasureService = unitOfMeasureService;
        }

        public List<UnitOfMeasureDto> Units { get; set; }

        public async Task<IActionResult> OnGetASync()
        {
            throw new NotImplementedException();
        }
    }
}
