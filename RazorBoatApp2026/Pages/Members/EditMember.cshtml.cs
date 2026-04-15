using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorMemberApp2026.Pages.Members
{
	public class EditMemberModel : PageModel
	{
		private IMemberRepositoryAsync _mRepo;
        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty] public Member NewMember { get; set; }
        [BindProperty] public IFormFile? Photo { get; set; }


		public EditMemberModel(IMemberRepositoryAsync memberRepository, IWebHostEnvironment webHostEnvironment)
        {
            _mRepo = memberRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGet(string phoneNumber)
		{
            string username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToPage("/Users/Login");
            }
            NewMember = await _mRepo.SearchAsync(phoneNumber);
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
                if (!string.IsNullOrEmpty(NewMember.MemberImage))
                {
                    string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "MemberImages", NewMember.MemberImage);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                NewMember.MemberImage = await ProcessUploadedFileAsync();
            }
            await _mRepo.UpdateAsync(NewMember);
			return RedirectToPage("Index");
		}
		public async Task<IActionResult> OnPostDelete()
		{
            if (!string.IsNullOrEmpty(NewMember.MemberImage))
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "MemberImages", NewMember.MemberImage);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            await _mRepo.RemoveAsync(NewMember.PhoneNumber);
            return RedirectToPage("Index");
        }

        private async Task<string> ProcessUploadedFileAsync()
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "MemberImages");
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
