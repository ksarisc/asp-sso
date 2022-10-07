using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspSso.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly Services.SigninManager signin;
        private readonly ILogger<AccountController> logger;
        //private readonly SignInManager<Models.ApplicationUser> signin;

        public AccountController(Services.SigninManager signinManager, ILogger<AccountController> accountLogger)
        {
            signin = signinManager;
            logger = accountLogger;
        }

        //public IActionResult Index() { return View(); }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Google([FromQuery] string? code = null)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                var token = await signin.ExchangeCodeAsync(code);
                return View(nameof(AfterLogin), token);
            }
            return View(nameof(AfterLogin));
        }

        [HttpGet]
        public IActionResult AfterLogin()
        {
            //foreach (var pair in Request.Query)
            //{
            //    System.Diagnostics.Debug.WriteLine("{0}: #{1}#", pair.Key, pair.Value);
            //}
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginForm model)
        //{
        //    var user = await AuthenticateUser(model.Email, model.Password);

        //    if (user == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //        return View();
        //    }

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.Email),
        //        new Claim("FullName", user.FullName),
        //        new Claim(ClaimTypes.Role, "Administrator"),
        //    };
        //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    var authProperties = new AuthenticationProperties
        //    {
        //        //AllowRefresh = <bool>,
        //        // Refreshing the authentication session should be allowed.

        //        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
        //        // The time at which the authentication ticket expires. A 
        //        // value set here overrides the ExpireTimeSpan option of 
        //        // CookieAuthenticationOptions set with AddCookie.

        //        //IsPersistent = true,
        //        // Whether the authentication session is persisted across 
        //        // multiple requests. When used with cookies, controls
        //        // whether the cookie's lifetime is absolute (matching the
        //        // lifetime of the authentication ticket) or session-based.

        //        //IssuedUtc = <DateTimeOffset>,
        //        // The time at which the authentication ticket was issued.

        //        //RedirectUri = <string>
        //        // The full path or absolute URI to be used as an http 
        //        // redirect response value.
        //    };

        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        //    logger.LogInformation("User {Email} logged in at {Time}.", user.Email, DateTime.UtcNow);

        //    return LocalRedirect(Url.GetLocalUrl(returnUrl));
        //}

        //public async Task Login(string returnUrl = "/"){
        //    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        //        .WithRedirectUri(returnUrl)
        //        .Build();

        //    await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        //}
    }
}
