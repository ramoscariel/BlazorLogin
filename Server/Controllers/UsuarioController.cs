using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorLogin.Shared;

namespace BlazorLogin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login) 
        {
            SesionDTO sesionDTO = new SesionDTO();
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }
            if (login.Correo == "admin@gmail.com" && login.Clave == "admin") 
            {
                sesionDTO.Nombre = "Administrador";
                sesionDTO.Correo = login.Correo;
                sesionDTO.Rol = "Administrador";
            }
            else 
            {
                sesionDTO.Nombre = "Usuario";
                sesionDTO.Correo = login.Correo;
                sesionDTO.Rol = "Empleado";
            }
            return StatusCode(StatusCodes.Status200OK, sesionDTO);
        }
    }
}
