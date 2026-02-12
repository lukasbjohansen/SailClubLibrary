using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Helpers.Sorting;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Boats
{
    public class IndexModel : PageModel
    {
		private IBoatRepository _bRepo;
		public List<Boat> Boats { get; set; }
		[BindProperty(SupportsGet = true)] public string SortBy { get; set; }
		[BindProperty(SupportsGet = true)] public string FilterCriteria { get; set; }
		public IndexModel(IBoatRepository boatRepository)
		{
			_bRepo = boatRepository;
		}
		public IActionResult OnGet()
        {
			if (!string.IsNullOrEmpty(FilterCriteria))
			{
				Boats = _bRepo.Filter(FilterCriteria);
			}
			else
			{
				Boats = _bRepo.GetAll();
				//Boats.Sort(new CompareById());
			}
			switch (SortBy)
			{
				case "Id":
					Boats.Sort(new CompareById());
					break;
			}
			return Page();
		}
		public IActionResult OnPostDelete(string sailNumber)
		{
			_bRepo.Remove(sailNumber);
			return RedirectToPage("Index");
		}
	}
}
