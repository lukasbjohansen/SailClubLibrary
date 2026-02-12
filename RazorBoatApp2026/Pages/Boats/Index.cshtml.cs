using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Boats
{
    public class IndexModel : PageModel
    {
		private IBoatRepository _bRepo;
		public List<Boat> Boats { get; set; }
		[BindProperty(SupportsGet = true)] public string SortBy { get; set; }
        [BindProperty(SupportsGet = true)] public bool SortDescending { get; set; }
        [BindProperty(SupportsGet = true)] public string FilterCriteria { get; set; }
		public IndexModel(IBoatRepository boatRepository)
		{
			_bRepo = boatRepository;
		}
		public IActionResult OnGet()
        {
            Boats = !string.IsNullOrEmpty(FilterCriteria) ? _bRepo.Filter(FilterCriteria) : _bRepo.GetAll();
            switch (SortBy)
            {
                case "BoatType":
                    Boats.Sort(new CompareByBoatType(SortDescending));
                    break;
                case "Model":
                    Boats.Sort(new CompareByBoatModel(SortDescending));
                    break;
                case "SailNumber":
                    Boats.Sort(new CompareByBoatSailNumber(SortDescending));
                    break;
                case "YearOfConstruction":
                    Boats.Sort(new CompareByBoatYearOfConstruction(SortDescending));
                    break;
                default:
                    Boats.Sort(new CompareById(SortDescending));
                    break;
            }
            return Page();
		}
		public IActionResult OnPostDelete(string sailNumber)
		{
			_bRepo.Remove(sailNumber);
			return RedirectToPage("Index");
		}
        public string GetSortIcon(string column)
        {
            if (SortBy != column) return "bi-arrows-expand";
            return SortDescending ? "bi-arrow-bar-up" : "bi-arrow-bar-down";
        }
    }
}
