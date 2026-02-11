using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Exceptions;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Members
{
	public class CreateMemberModel : PageModel
	{
		private IMemberRepository _mRepo;
		[BindProperty] public Member NewMember { get; set; }
		public CreateMemberModel(IMemberRepository memberRepository)
		{
			_mRepo = memberRepository;
		}
		public void OnGet()
		{
			NewMember = new Member();
			NewMember.Id = _mRepo.SearchLowestNotTakenId();
		}
		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
			{
				ViewData["ErrorMessage"] = "Every field must be filled";
				return Page();
			}
			try
			{
				_mRepo.Add(NewMember);
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
	}
}
