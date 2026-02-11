using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Members
{
	public class IndexModel : PageModel
	{
		private IMemberRepository _mRepo;
		public List<Member> Members{ get; set; }
		public IndexModel(IMemberRepository memberRepository)
		{
			_mRepo = memberRepository;
		}
		public void OnGet()
		{
			Members = _mRepo.GetAll().OrderBy(b => b.Id).ToList();
		}
		public IActionResult OnPostDelete(string phoneNumber)
		{
			_mRepo.Remove(phoneNumber);
			return RedirectToPage("Index");
		}
	}
}
