using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Boats
{
    public class EditBoatModel : PageModel
    {
        private IBoatRepositoryAsync _bRepo;
        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty] public Boat NewBoat { get; set; }
        [BindProperty] public IFormFile? Photo { get; set; }

        public EditBoatModel(IBoatRepositoryAsync boatRepositoryAsync, IWebHostEnvironment webHostEnvironment)
        {
            _bRepo = boatRepositoryAsync;
            _webHostEnvironment = webHostEnvironment;
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
            if (Photo != null)
            {
                if (!string.IsNullOrEmpty(NewBoat.BoatImage))
                {
                    string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "BoatImages", NewBoat.BoatImage);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                NewBoat.BoatImage = await ProcessUploadedFileAsync();
            }
            await _bRepo.UpdateAsync(NewBoat);
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostDelete()
        {
            await _bRepo.RemoveAsync(NewBoat.SailNumber);
            return RedirectToPage("Index");
        }
        private async Task<string> ProcessUploadedFileAsync()
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "BoatImages");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await Photo.CopyToAsync(fileStream);
            }
            return uniqueFileName;
        }
    }
}
