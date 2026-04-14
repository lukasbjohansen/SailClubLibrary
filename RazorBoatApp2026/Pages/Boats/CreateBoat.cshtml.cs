using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Exceptions;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Boats
{
    public class CreateBoatModel : PageModel
    {
		private IBoatRepositoryAsync _bRepo;
		[BindProperty] public Boat NewBoat { get; set; }
		public CreateBoatModel(IBoatRepositoryAsync boatRepositoryAsync)
		{
			_bRepo = boatRepositoryAsync;
		}
		public async Task OnGet()
        {
			NewBoat = new Boat();
			NewBoat.Id = await _bRepo.SearchLowestNotTakenIdAsync();
		}
		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			try
			{
				await _bRepo.AddAsync(NewBoat);
				return RedirectToPage("Index");
			}
			catch (BoatSailnumberExistsException bex)
			{
				ViewData["ErrorMessage"] = bex.Message;
				return Page();
			}
			catch (Exception e)
			{
				ViewData["ErrorMessage"] = e.Message;
				return Page();
			}
		}
    }
}
