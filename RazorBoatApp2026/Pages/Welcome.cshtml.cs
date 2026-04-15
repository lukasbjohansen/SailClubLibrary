using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorBoatApp2026.Pages
{
    public class WelcomeModel : PageModel
    {
        public string Username { get; set; }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            if (Username == null)
            {
                return RedirectToPage("/Users/Login");
            }
            return Page();
        }
    }
}
