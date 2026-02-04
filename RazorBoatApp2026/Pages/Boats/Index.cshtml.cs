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
		public IndexModel(IBoatRepository boatRepository)
		{
			_bRepo = boatRepository;
		}
		public void OnGet()
        {
			Boats = _bRepo.GetAllBoats();
        }
    }
}
