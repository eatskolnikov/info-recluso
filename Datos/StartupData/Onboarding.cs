using Datos.Data;
using Datos.Utilities;

namespace Datos.StartupData;

public class Onboarding
{
    private readonly AccountServices _accountManager;

    public Onboarding() => _accountManager = Injector.GetService<AccountServices>();

    protected async Task<Response> AddRolesAsync()
    {
        try
        {
            await _accountManager.CreateAllRoles();
            return Response.Ok();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }

    protected async Task<Response> AddDeveloperUser()
    {
        try
        {
            var accountMicroServices = Injector.GetService<AccountServices>();

            //Instance
            var users = new List<UserDto>()
            {
                new() { Email = "dev@inforecluso.com", Pass = "Dev123456*", UserName = "Developer", PhoneNumber = "809-000-0000" }
            };

            // add all users 
            foreach (var user in users) await accountMicroServices.UserCreateAsync(user);

            // addRoles
            foreach (var user in users)
            {
                var roles = Roles.RolesList.Where(x => user.Email.ToLower().Contains(x.ToString())).ToList();
                await accountMicroServices.UserAddsRolesAsync(user, roles);
            }

            return Response.Ok();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }

    public  async Task<Response> StartData()
    {
        try
        {
            await AddRolesAsync();
            await AddDeveloperUser();
            return Response.Ok();
        }
        catch (Exception ex)
        {
            return Response.Error(ex.Message);
        }
    }
}