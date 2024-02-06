using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using IdentityModel;
using Marvin.IDP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marvin.IDP.Pages.User.Registration
{
    [AllowAnonymous]
    [SecurityHeaders] //Agrega encabezados relacionados con la seguridad de la respuesta
    public class IndexModel : PageModel
    {

        private readonly ILocalUserService _localUserService;
        private readonly IIdentityServerInteractionService _interactionService;

        public IndexModel(ILocalUserService localUserService, IIdentityServerInteractionService interactionService)
        {
            _localUserService = localUserService ?? throw new ArgumentNullException(nameof(localUserService));
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet(string returnUrl)
        {
            BuildModel(returnUrl);

            return Page();
        }

        private void BuildModel(string returnUrl)
        {
            Input = new InputModel
            {
                ReturnUrl = returnUrl,
            };
        }


        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                BuildModel(Input.ReturnUrl);
                return Page();
            };

            Entities.User userToCreate = new()
            {
                Active = false,
                Subject = Guid.NewGuid().ToString(),
                UserName = Input.UserName,
                Email = Input.Email,
            };

            userToCreate.Claims.Add(new()
            {
                Type = "country",
                Value = Input.Country,
            });
            userToCreate.Claims.Add(new()
            {
                Type = JwtClaimTypes.GivenName,
                Value = Input.GivenName
            });
            userToCreate.Claims.Add(new()
            {
                Type = JwtClaimTypes.FamilyName,
                Value = Input.FamilyName
            });

            _localUserService.AddUser(userToCreate, Input.Password);
            await _localUserService.SaveChangesAsync();

            //Create an activation link
            var activationLink = Url.PageLink("/User/Activation/Index", values: new { securityCode = userToCreate.SecurityCode });

            Console.WriteLine("------ ACTIVATION LINK -------");
            Console.WriteLine(activationLink.ToString());

            return Redirect("~/User/ActivationCodeSent");

            /**COMO NO QUEREMOS QUE EL USUARIO INICIE SESION CUANDO SE REGISTRA PORQUE ANTES DEBE VALIDAR SU CORREO, COMENTAMOS EL SIGUIENTE CODIGO**/

            ////Creamos un IdentityServerUser para poder iniciar sesion
            //var isUser = new IdentityServerUser(userToCreate.Subject)
            //{
            //    DisplayName = userToCreate.UserName
            //};

            ////Iniciamos sesion con dicho usuario
            //await HttpContext.SignInAsync(isUser);

            ////Contnuamos con el flujo
            //if (_interactionService.IsValidReturnUrl(Input.ReturnUrl) || Url.IsLocalUrl(Input.ReturnUrl))
            //{
            //    return Redirect(Input.ReturnUrl);
            //}

            //return Redirect("~/");

        }
    }
}
