using Datos.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;
using static Datos.Utilities.Roles;
namespace Datos.Data;

public class AccountServices
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountServices(UserManager<IdentityUser> userManager,
                             SignInManager<IdentityUser> signInManager,
                             RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
    public async Task<(String token, Response response)> UserCreateAsync(UserDto userDto)
    {
        try
        {
            var identityUser = (IdentityUser)userDto;
            var result = await _userManager.CreateAsync(identityUser, userDto.Pass);

            if (result.Succeeded is true)
                return (await UtilServices.TokenGenerator(userDto), Response.Ok());

            return (default, Response.Fail(message: "Error al crear usuario"));
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.Message));
        }
    }

    public async Task<(String token, Response response)> UserCreateAsync(UserDto userDto, string Role)
    {
        try
        {
            var identityUser = (IdentityUser)userDto;
            var result = await _userManager.CreateAsync(identityUser, userDto.Pass);

            if (result.Succeeded is true)
            {
                var roles = Roles.RolesList.Where(x => x.ToString() == $"{Role.ToLower()}").ToList();
                await UserAddsRolesAsync(userDto, roles);

                return (await UtilServices.TokenGenerator(userDto), Response.Ok());
            }

            return (default, Response.Fail(message: "Error al crear usuario"));
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.Message));
        }
    }

    public async Task<Response> UserDel(IdentityUser userDto)
    {
        try
        {
            if (userDto.Email.Trim().ToLower() == "dev@inforecluso.com")
                return Response.Fail("Este usuario no se puede eliminar");

            var user = await _userManager.FindByEmailAsync(userDto.Email); 
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded is true)
                return Response.Ok("Usuario Eliminado");

            return Response.Fail(message: "Error al crear borrar");
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }

    public async Task<Response> ResetPass(IdentityUser userDto, string pass)
    {
        try
        {
            if (userDto.Email.ToLower() == "dev@gmail.com")
                return Response.Fail("Este usuario no se puede cambiar");

            var user = await _userManager.FindByEmailAsync(userDto.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, pass);

                if (result.Succeeded)
                    return Response.Ok("Clave Reseteada");
            }

            return Response.Fail("No se ha podido proceder");
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }

    public (IEnumerable<IdentityUser>, Response response) GetAllUsers()
    {
        try
        {
            var result = _userManager.Users.ToList();
            return (result, result.Count == 0 ? Response.NoFound() : Response.Ok());
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.Message));
        }
    }

    public async Task<string> GetUserRole(IdentityUser user)
    {
        var roles = await _userManager?.GetRolesAsync(user);
        return roles.FirstOrDefault();
    }

    public async Task<(String token, Response response)> UserSignInAsync(UserDto userDto)
    {
        try
        {
            SignInResult signInResult = default;
            var user = await _userManager.FindByEmailAsync(userDto.Email);

            if (user is not null)
                signInResult = await _signInManager.CheckPasswordSignInAsync(user, userDto.Pass, false);

            if (signInResult is not null && signInResult.Succeeded is true)
                return (await UtilServices.TokenGenerator(userDto: userDto, userManager: _userManager), Response.Ok());

            return (default, Response.Fail("Los datos suministrado no son correctos"));
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.Message));
        }
    }

    public async Task UserAddsRolesAsync(IdentityUser user, List<UserRoles> roles)
    {
        var userGet = await _userManager.FindByEmailAsync(user.Email);

        foreach (var item in roles)
            await _userManager.AddToRoleAsync(userGet, item.ToString());
    }

    public async Task UserAddClaimAsync(IdentityUser user, List<UserRoles> roles)
    {
        foreach (var item in roles)
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, item.ToString()));
    }

    public async Task UserRemoveClaimAsync(IdentityUser user, List<UserRoles> roles)
    {
        foreach (var item in roles)
            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, item.ToString()));
    }

    public async Task<Response> CreateAllRoles()
    {
        try
        {
            foreach (var item in RolesList)
                await _roleManager.CreateAsync(new IdentityRole() { Name = item.ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });

            return Response.Ok();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }
}