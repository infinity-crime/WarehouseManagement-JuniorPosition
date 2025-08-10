using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WarehouseManagement.Application.Common;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.Web.Pages.Directories.Resources
{
    public class FormModel : PageModel
    {
        private readonly IResourceService _resourceService;

        public FormModel(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public string Input { get; set; } = string.Empty;

        public ResourceDto? CurrentResource { get; set; }

        public bool IsNewResource => Id == Guid.Empty;
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (IsNewResource)
                return Page();

            var result = await _resourceService.GetByIdAsync(Id);
            if(result.IsSucces && result.Value != null)
            {
                Input = result.Value!.Name;
                CurrentResource = result.Value;
                return Page();
            }

            ErrorMessage = result.Error;
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (IsNewResource)
                return await CreateNewResource();

            return await UpdateExistingResource();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var result = await _resourceService.DeleteAsync(Id);
            if(result.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = result.Error;
            return Page();
        }

        public async Task<IActionResult> OnPostArchiveAsync()
        {
            var result = await _resourceService.MoveToArchiveAsync(Id);
            if(result.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = result.Error;
            return Page();
        }

        public async Task<IActionResult> OnPostToWorkAsync()
        {
            var result = await _resourceService.MoveToWorkAsync(Id);
            if (result.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = result.Error;
            return Page();
        }

        private async Task<IActionResult> CreateNewResource()
        {
            var result = await _resourceService.CreateAsync(Input);
            if (result.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = result.Error;
            return Page();
        }

        private async Task<IActionResult> UpdateExistingResource()
        {
            var resultUpdate = await _resourceService.ChangeNameAsync(Id, Input);
            if(resultUpdate.IsSucces)
                return RedirectToPage("Main");

            ErrorMessage = resultUpdate.Error;
            return Page();
        }
    }
}
