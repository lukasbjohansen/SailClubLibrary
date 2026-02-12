using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;
using System.Globalization;

namespace RazorBoatApp2026.Pages.Members
{
	public class IndexModel : PageModel
	{
		private IMemberRepository _mRepo;
		public List<Member> Members{ get; set; }
        [BindProperty(SupportsGet = true)] public string SortBy { get; set; }
        [BindProperty(SupportsGet = true)] public bool SortDescending { get; set; }
        [BindProperty(SupportsGet = true)] public string FilterCriteria { get; set; }
        public IndexModel(IMemberRepository memberRepository)
		{
			_mRepo = memberRepository;
		}
        public IActionResult OnGet()
        {
            Members = !string.IsNullOrEmpty(FilterCriteria) ? _mRepo.Filter(FilterCriteria) : _mRepo.GetAll();
            switch (SortBy)
            {
                case "Name":
                    Members.Sort(new CompareByMemberName(SortDescending));
                    break;
                case "Address":
                    Members.Sort(new CompareByMemberAddress(SortDescending));
                    break;
                case "Phone":
                    Members.Sort(new CompareByMemberPhone(SortDescending));
                    break;
                case "Mail":
                    Members.Sort(new CompareByMemberMail(SortDescending));
                    break;
                default:
                    Members.Sort(new CompareById(SortDescending));
                    break;
            }
            return Page();
        }
        public IActionResult OnPostDelete(string phoneNumber)
		{
			_mRepo.Remove(phoneNumber);
			return RedirectToPage("Index");
		}
        public string GetSortIcon(string column)
        {
            if (SortBy != column) return "bi-arrows-expand";
            return SortDescending ? "bi-arrow-bar-up" : "bi-arrow-bar-down";
        }
    }
}
