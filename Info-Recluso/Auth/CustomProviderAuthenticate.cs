using Auth;
using Blazored.LocalStorage;
using Datos.Extensions;
using Datos.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;

namespace Info_Recluso.Auth;

public class CustomProviderAuthenticate : AuthenticationStateProvider
{
    private readonly AccountStorages tokenStorages;

    private AuthenticationState Anonimo =>
      new AuthenticationState
      (new ClaimsPrincipal(new ClaimsIdentity
          (new List<Claim>() { new Claim(ClaimTypes.Role, Roles.UserRoles.anonymous.ToString()) }, Roles.UserRoles.anonymous.ToString())));

    public CustomProviderAuthenticate(AccountStorages tokenStorages)
    {
        this.tokenStorages = tokenStorages;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await tokenStorages.GetToken();
            if (string.IsNullOrWhiteSpace(token))
                return await Task.FromResult(Anonimo);

            var userGet = await SectionBuild(token);
            return await Task.FromResult(userGet);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(Anonimo);
        }
    }
    public async Task LogIn(string token)
    {
        await tokenStorages.SetToken(token);
        var userGet = await SectionBuild(token);
        NotifyAuthenticationStateChanged(Task.FromResult(userGet));

    }
    public async Task LogOut()
    {
        await tokenStorages.ClearAll();
        NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
    }
    public async Task<AuthenticationState> SectionBuild(string token)
    {
        if (string.IsNullOrEmpty(token))
            return Anonimo;

        var result = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsToken.ParseClaimsFromJwt(token), "Auth")));
        return await Task.FromResult(result);
    }
}