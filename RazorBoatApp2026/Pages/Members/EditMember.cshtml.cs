using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorMemberApp2026.Pages.Members
{
	public class EditMemberModel : PageModel
	{
		private IMemberRepository _mRepo;
		[BindProperty] public Member NewMember { get; set; }
		public EditMemberModel(IMemberRepository memberRepository)
		{
			_mRepo = memberRepository;
		}
		public void OnGet(string phoneNumber)
		{
			NewMember = _mRepo.Search(phoneNumber);
		}
		public IActionResult OnPost()
		{
			_mRepo.Update(NewMember);
			return RedirectToPage("Index");
		}
		public IActionResult OnPostDelete()
		{
			_mRepo.Remove(NewMember.PhoneNumber);
			return RedirectToPage("Index");
		}
	}
}
