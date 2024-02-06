using Marvin.IDP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marvin.IDP.Pages.User.Activation
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class IndexModel : PageModel
    {

        private readonly ILocalUserService _localUserService;

        public IndexModel(ILocalUserService localUserService)
        {
            _localUserService = localUserService ?? throw new ArgumentNullException(nameof(localUserService));
            Input = new InputModel { Message = "" };
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public async Task<IActionResult> OnGet(string securityCode)
        {
            if (await _localUserService.ActivateUserAsync(securityCode))
            {
                Input.Message = "Your Account was successfull activated." + "Navigate to your client application to log in.";
            }
            else
            {
                Input.Message = "Your Account couldn�t be activated." + "Please contect your administrator.";
            }

            await _localUserService.SaveChangesAsync();

            return Page();
        }
    }
}
