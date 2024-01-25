using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorization
{
    public class MustOwnImageRequirement : IAuthorizationRequirement
    {
        //PAra lo que se puede usar esta clase es para almacenar informacion contextual adicional:
        //EJ:
        //Si debemos hacer un requisito que establezca que un usuario debe ser de un país especifica podriamos ingresar el valor de país como parametro del contructor para que coincida
        public MustOwnImageRequirement() { } //--> Para esta validacion no hay necesidad de agregar ningun valor

    }
}
