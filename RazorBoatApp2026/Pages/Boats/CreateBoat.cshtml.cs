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
		private IWebHostEnvironment _webHostEnvironment;

        [BindProperty] public Boat NewBoat { get; set; }
        [BindProperty] public IFormFile Photo { get; set; }

        public CreateBoatModel(IBoatRepositoryAsync boatRepositoryAsync, IWebHostEnvironment webHostEnvironment)
		{
			_bRepo = boatRepositoryAsync;
			_webHostEnvironment = webHostEnvironment;
		}
		public async Task<IActionResult> OnGet()
        {
            string username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToPage("/Users/Login");
            }
            NewBoat = new Boat();
			NewBoat.Id = await _bRepo.SearchLowestNotTakenIdAsync();
            return Page();
		}
		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
            if (Photo != null)
            {
                if (NewBoat.BoatImage != null)
                {
                    string filepath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "BoatImages", NewBoat.BoatImage);
                    System.IO.File.Delete(filepath);
                }
                NewBoat.BoatImage = await ProcessUploadedFileAsync();
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
        private async Task<string> ProcessUploadedFileAsync()
        {
            string uniqueFileName = null;
            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/BoatImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
