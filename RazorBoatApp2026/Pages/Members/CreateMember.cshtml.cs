using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Exceptions;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Members
{
	public class CreateMemberModel : PageModel
	{
		private IMemberRepositoryAsync _mRepo;
        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty] public Member NewMember { get; set; }
		[BindProperty] public IFormFile Photo { get; set; }
		public CreateMemberModel(IMemberRepositoryAsync memberRepository, IWebHostEnvironment webHostEnvironment)
		{
			_mRepo = memberRepository;
			_webHostEnvironment = webHostEnvironment;
		}
		public async Task OnGet()
		{
			NewMember = new Member();
			NewMember.Id = await _mRepo.SearchLowestNotTakenIdAsync();
		}
		public async Task<IActionResult> OnPost()
		{
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Photo != null)
			{
				if (NewMember.MemberImage != null)
				{
					string filepath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "MemberImages", NewMember.MemberImage);
					System.IO.File.Delete(filepath);
				}
				NewMember.MemberImage = await ProcessUploadedFileAsync();
			}
			try
			{
				await _mRepo.AddAsync(NewMember);
				return RedirectToPage("Index");
			}
			catch (BoatSailnumberExistsException bex)
			{
				ViewData["ErrorMessage"] = bex.Message;
				return Page();
			}
            catch (ArgumentException aex)
            {
                ViewData["ErrorMessage"] = aex.Message;
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
				string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/MemberImages");
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
