using Auth;
using Datos.Data;
using Datos.Dtos;
using Datos.Entities;
using Datos.Generic;
using Datos.Interfaces;
using Datos.StartupData;
using Datos.Utilities;
using Info_Recluso.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using System.Security.Claims;
using Utilities.FileHelper;
public class Startup
{
    public static IConfiguration configuration { get; private set; }
    public static IServiceProvider serviceProvider { get; private set; }
    public Startup(IConfiguration _configuration) => configuration = _configuration;
    public void ConfigureServices(IServiceCollection services)
    {
        #region  principales
        services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });
        services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        services.AddAuthenticationCore();
        services.AddRazorPages();
        services.AddServerSideBlazor(opt => { });
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
            config.SnackbarConfiguration.PreventDuplicates = true;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Text;
            config.SnackbarConfiguration.BackgroundBlurred = false;
            config.SnackbarConfiguration.MaxDisplayedSnackbars = 3;
        });
        services.AddAutoMapper(typeof(AutoMapperProfiles));
        #endregion

        services.AddScoped<Repository>();
        services.AddScoped<AccountStorages>();
        services.AddScoped<AccountServices>();
        services.AddScoped<IPrisonerServices, PrisonerServices>();
        services.AddScoped<CustomProviderAuthenticate>();
        services.AddScoped<AuthenticationStateProvider, CustomProviderAuthenticate>();
        services.AddScoped<IFileManager, FileManager>();

        services.AddAuthentication("Cookies").AddCookie(opt =>
        {
            opt.Cookie.Name = "TryingoutGoogleOAuth";
            opt.LoginPath = "/auth/google-login";
            opt.Cookie.IsEssential = true;
            opt.Cookie.HttpOnly = false;
            opt.Cookie.Expiration = TimeSpan.FromDays(365);
        }).AddGoogle(opt =>
        {
            opt.ClientId = configuration["Google:Id"];
            opt.ClientSecret = configuration["Google:Secret"];
            opt.Scope.Add("profile");

            opt.Events.OnCreatingTicket = async context =>
            {
                string photoUrl = context.User.GetProperty("picture").GetString();
                context.Identity.AddClaim(new Claim("Picture", photoUrl));
                context.Identity.AddClaim(new Claim($"{ClaimTypes.Role}", Roles.UserRoles.google.ToString()));
                var email = context.Identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var userGoogle = new UserDto { Email = email, UserName = email, Pass = string.Empty };
                var token = await UtilServices.TokenGenerator(userDto: userGoogle, listClaims: context.Identity.Claims.ToList());
                var httpContextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
            };
        });

        //Para conseguir inyecciones sin inject y constructor
        Injector.GenerarProveedor(services);

        // Instance
        var onboarding = new Onboarding();
        // Start
        _ = onboarding.StartData();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IServiceProvider service)
    {
        serviceProvider = service;
        bool EnableLimit = bool.Parse(configuration["UserLimit:Enable"].ToString());
        int RequestxTime = int.Parse(configuration["UserLimit:Request"].ToString());
        int TimeSecond = int.Parse(configuration["UserLimit:Seconds"]);

        // Configure the HTTP request pipeline.
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseEndpoints(endpoins => { endpoins.MapControllers(); });
    }
}