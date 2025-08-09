using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.Web.Pages.Directories.Resources
{
    public class MainModel : PageModel
    {
        private readonly IResourceService _resourceService;

        public MainModel(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public List<ResourceDto> Resources { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var result = await _resourceService.GetAllActiveResourcesAsync();
            Resources = result.Value!.ToList();

            return Page();
        }
    }
}
