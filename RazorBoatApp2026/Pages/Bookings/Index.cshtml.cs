using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;
using System.Globalization;

namespace RazorBoatApp2026.Pages.Bookings
{
	public class IndexModel : PageModel
	{
		private IBookingRepositoryAsync _bRepo;
		public List<Booking> Bookings { get; set; }
		[BindProperty(SupportsGet = true)] public string SortBy { get; set; }
		[BindProperty(SupportsGet = true)] public bool SortDescending { get; set; }
		[BindProperty(SupportsGet = true)] public string FilterCriteria { get; set; }
		public IndexModel(IBookingRepositoryAsync bookingRepository)
		{
			_bRepo = bookingRepository;
		}
		public async Task<IActionResult> OnGet()
		{
            string username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToPage("/Users/Login");
            }
            Bookings = !string.IsNullOrEmpty(FilterCriteria) ? await _bRepo.FilterAsync(FilterCriteria) : await _bRepo.GetAllAsync();
			switch (SortBy)
			{
				case "StartDate":
					Bookings.Sort(new CompareByBookingStartDate(SortDescending));
					break;
				case "EndDate":
					Bookings.Sort(new CompareByBookingEndDate(SortDescending));
					break;
				case "Destination":
					Bookings.Sort(new CompareByBookingDestination(SortDescending));
					break;
				case "Member":
					Bookings.Sort(new CompareByBookingMember(SortDescending));
					break;
				case "Boat":
					Bookings.Sort(new CompareByBookingBoat(SortDescending));
					break;
				default:
					Bookings.Sort(new CompareById(SortDescending));
					break;
			}
			return Page();
		}
		public async Task<IActionResult> OnPostDelete(int id)
		{
			await _bRepo.RemoveAsync(id);
			return RedirectToPage("Index");
		}
		public string GetSortIcon(string column)
		{
			if (SortBy != column)
				return "bi-arrows-expand";
			return SortDescending ? "bi-arrow-bar-up" : "bi-arrow-bar-down";
		}
	}
}
