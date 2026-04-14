using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Boats
{
    public class EditBoatModel : PageModel
    {
        private IBoatRepositoryAsync _bRepo;
        [BindProperty] public Boat NewBoat { get; set; }
        public EditBoatModel(IBoatRepositoryAsync boatRepositoryAsync)
        {
            _bRepo = boatRepositoryAsync;
        }
        public async Task OnGet(string sailNumber)
        {
            NewBoat = await _bRepo.SearchAsync(sailNumber)!;
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _bRepo.UpdateAsync(NewBoat);
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostDelete()
        {
            await _bRepo.RemoveAsync(NewBoat.SailNumber);
            return RedirectToPage("Index");
        }
    }
}
