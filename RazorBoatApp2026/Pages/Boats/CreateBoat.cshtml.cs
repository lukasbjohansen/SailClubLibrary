using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        }
		public IActionResult OnPost()
		{
			NewBoat.Id = _bRepo.SearchLowestNotTakenId();
			_bRepo.AddBoat(NewBoat);
			return RedirectToPage("Index");
		}
    }
}
