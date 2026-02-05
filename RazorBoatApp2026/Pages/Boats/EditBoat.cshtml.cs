using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Boats
{
    public class EditBoatModel : PageModel
    {
        private IBoatRepository _bRepo;
        [BindProperty] public Boat NewBoat { get; set; }
        public EditBoatModel(IBoatRepository boatRepository)
        {
            _bRepo = boatRepository;
        }
        public void OnGet(string sailNumber)
        {
            NewBoat = _bRepo.SearchBoat(sailNumber);
        }
        public IActionResult OnPost()
        {
            _bRepo.UpdateBoat(NewBoat);
            return RedirectToPage("Index");
        }
        public IActionResult OnPostDelete()
        {
            _bRepo.RemoveBoat(NewBoat.SailNumber);
            return RedirectToPage("Index");
        }
    }
}
