using ImageGallery.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorization
{
    public class MustOwnImageHandler : AuthorizationHandler<MustOwnImageRequirement>
    {
        //OBJETIVO DE ESTE HANDLER:
        //Que devuelva un resultado de aprobado o desaprobado de la validacion del requerimiento (MustOwnImageRequirement)
        // Se debe probar: Que el ID de la  image que llega en el request pertenezca al usuario que se logueó en la app cliente.

        private readonly IHttpContextAccessor _httpContextAccessor; //Debemos agregar AddHttContextAccessor en Program.cs
        private readonly IGalleryRepository _galleryRepository;

        public MustOwnImageHandler(IHttpContextAccessor httpContextAccessor, IGalleryRepository galleryRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _galleryRepository = galleryRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            MustOwnImageRequirement requirement)
        {
            //Obtenemos el id de la image de la ruta del request
            var imageId = _httpContextAccessor.HttpContext?.GetRouteValue("id")?.ToString();

            //Intentamos analizar en un GUID
            if(!Guid.TryParse(imageId, out var imageIdAsGuid)){
                context.Fail(); //Desaprobamos el requerimiento
                return;
            }

            //Obtenemos el id del usuario, para buscarlo en la base de datos:
            var ownerId = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if(ownerId == null)
            {
                context.Fail(); //Desaprobamos el requerimiento.
                return;
            }

            //Identificamos si la imagen pertenece al usuario
            var result = await _galleryRepository.IsImageOwnerAsync(imageIdAsGuid, ownerId);
            if(result == false)
            {
                context.Fail(); //Desaprobamos el requerimiento.
                return;
            }

            context.Succeed(requirement); //Aprobamos el requerimiento

        }
    }
}
