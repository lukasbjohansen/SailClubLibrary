using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SailClubLibrary.Interfaces;
using SailClubLibrary.Models;

namespace RazorBoatApp2026.Pages.Users
{
    public class LoginModel : PageModel
    {
        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }

        public string Message { get; set; }

        private IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            if (Username == null)
            {
                return Page();
            }
            return RedirectToPage("/Welcome");
        }

        public void OnGetLogout()
        {
            HttpContext.Session.Remove("Username");
        }

        public IActionResult OnPost()
        {
            User loginUser = _userService.VerifyUser(Username, Password);
            if (loginUser != null)
            {
                HttpContext.Session.SetString("Username", loginUser.Username);
                return RedirectToPage("/Welcome");
            }
            Message = "Invalid username or password";
            Username = "";
            Password = "";
            return Page();
        }
    }
}
