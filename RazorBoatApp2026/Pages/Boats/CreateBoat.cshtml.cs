using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Exceptions;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Boats
{
    public class CreateBoatModel : PageModel
    {
		private IBoatRepository _bRepo;
		[BindProperty] public Boat NewBoat { get; set; }
		public CreateBoatModel(IBoatRepository boatRepository)
		{
			_bRepo = boatRepository;
		}
		public void OnGet()
        {
			NewBoat = new Boat();
			NewBoat.Id = _bRepo.SearchLowestNotTakenId();
		}
		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			try
			{
				_bRepo.Add(NewBoat);
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
