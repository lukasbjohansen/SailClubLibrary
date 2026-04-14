using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SailClubLibrary.Exceptions;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorBoatApp2026.Pages.Bookings
{
	public class CreateBookingModel : PageModel
	{
		private IBookingRepositoryAsync _bookingRepo;
		private IMemberRepositoryAsync _memberRepo;
		private IBoatRepositoryAsync _boatRepo;
		[BindProperty] public Booking NewBooking { get; set; }
		[BindProperty] [Required(ErrorMessage = "Member selection is required")] public string SelectedMemberPhoneNumber { get; set; }

		[BindProperty] [Required(ErrorMessage = "Boat selection is required")] public string SelectedBoatSailNumber { get; set; }
		public SelectList MemberOptions { get; set; }
		public SelectList BoatOptions { get; set; }
		public CreateBookingModel(IMemberRepositoryAsync memberRepository, IBoatRepositoryAsync boatRepository, IBookingRepositoryAsync bookingRepository)
		{
			_memberRepo = memberRepository;
			_boatRepo = boatRepository;
			_bookingRepo = bookingRepository;
		}
		public async Task OnGet()
		{
			NewBooking = new Booking()
			{
				Id = await _bookingRepo.SearchLowestNotTakenIdAsync(),
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddDays(1),
				SailCompleted = false
			};
			PopulateLists();
		}
		public async Task<IActionResult> OnPost()
		{
			ModelState.Remove("NewBooking.TheMember");
			ModelState.Remove("NewBooking.TheBoat");
			if (SelectedBoatSailNumber == null || SelectedMemberPhoneNumber == null)
			{
				PopulateLists();
				return Page();
			}
			if (!ModelState.IsValid)
			{
				PopulateLists();
				return Page();
			}
			NewBooking.TheMember = await _memberRepo.SearchAsync(SelectedMemberPhoneNumber);
			NewBooking.TheBoat = await _boatRepo.SearchAsync(SelectedBoatSailNumber);
			try
			{
				await _bookingRepo.AddAsync(NewBooking);
				return RedirectToPage("Index");
			}
			catch (BoatSailnumberExistsException bex)
			{
				ViewData["ErrorMessage"] = bex.Message;
				PopulateLists();
				return Page();
			}
			catch (Exception e)
			{
				ViewData["ErrorMessage"] = e.Message;
				PopulateLists();
				return Page();
			}
		}
		private async Task PopulateLists()
		{
			MemberOptions = new SelectList(await _memberRepo.GetAllAsync(), "PhoneNumber", "FullName");
			BoatOptions = new SelectList(await _boatRepo.GetAllAsync(), "SailNumber", "");
		}
	}
}
