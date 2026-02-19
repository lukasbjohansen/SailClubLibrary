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
		private IBookingRepository _bookingRepo;
		private IMemberRepository _memberRepo;
		private IBoatRepository _boatRepo;
		[BindProperty] public Booking NewBooking { get; set; }
		[BindProperty] [Required(ErrorMessage = "Member selection is required")] public string SelectedMemberPhoneNumber { get; set; }

		[BindProperty] [Required(ErrorMessage = "Boat selection is required")] public string SelectedBoatSailNumber { get; set; }
		public SelectList MemberOptions { get; set; }
		public SelectList BoatOptions { get; set; }
		public CreateBookingModel(IMemberRepository memberRepository, IBoatRepository boatRepository, IBookingRepository bookingRepository)
		{
			_memberRepo = memberRepository;
			_boatRepo = boatRepository;
			_bookingRepo = bookingRepository;
		}
		public void OnGet()
		{
			NewBooking = new Booking()
			{
				Id = _bookingRepo.SearchLowestNotTakenId(),
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddDays(1),
				SailCompleted = false
			};
			PopulateLists();
		}
		public IActionResult OnPost()
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
			NewBooking.TheMember = _memberRepo.Search(SelectedMemberPhoneNumber);
			NewBooking.TheBoat = _boatRepo.Search(SelectedBoatSailNumber);
			try
			{
				_bookingRepo.Add(NewBooking);
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
		private void PopulateLists()
		{
			MemberOptions = new SelectList(_memberRepo.GetAll(), "PhoneNumber", "FullName");
			BoatOptions = new SelectList(_boatRepo.GetAll(), "SailNumber");
		}
	}
}
