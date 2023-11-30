using BlazorLogin.Shared;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorLogin.Client.Extensiones
{
    public class AutenticacionExtension : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorage;
        private ClaimsPrincipal _sinInfor = new ClaimsPrincipal(new ClaimsIdentity());
        public AutenticacionExtension(ISessionStorageService _sessionStorage)
        {
            this._sessionStorage = _sessionStorage;
        }

        public async Task ActualizarEstadoAutenticacion(SesionDTO? sesionUsuario) 
        {
            ClaimsPrincipal claimsPrincipal;
            if(sesionUsuario != null) 
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name,sesionUsuario.Nombre),
                    new Claim(ClaimTypes.Email,sesionUsuario.Correo),
                    new Claim(ClaimTypes.Role,sesionUsuario.Rol)
                },"JwtAuth"));
                await _sessionStorage.GuardarStorage("sesionUsuario", sesionUsuario);
            }
            else 
            {
                claimsPrincipal = _sinInfor;
                await _sessionStorage.RemoveItemAsync("sesionUsuario");
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var sesionUsuario = await _sessionStorage.ObtenerStorage<SesionDTO>("sesionUsuario");
            if (sesionUsuario == null) 
            {
                return await Task.FromResult(new AuthenticationState(_sinInfor));
            }
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name,sesionUsuario.Nombre),
                    new Claim(ClaimTypes.Email,sesionUsuario.Correo),
                    new Claim(ClaimTypes.Role,sesionUsuario.Rol)
                }, "JwtAuth"));
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
    }
}
