using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;

using Mmicovic.RPSSL.Service;
using Mmicovic.RPSSL.API.Models;
using Mmicovic.RPSSL.API.Validators;
using Mmicovic.RPSSL.API.Exceptions;
using Mmicovic.RPSSL.API.Initialization;

namespace Mmicovic.RPSSL.API.Controllers
{
    [Route("api")]
    [EnableCors]
    [ApiController]
    /* This controller holds endpoints for logging in and registration.
     * It relays logging and registration requests to the UserManager service.
     * If logging or registration are successful, it generates a JWT token for the response.*/
    public class LoginController(IUserManager userManager, IConfiguration configuration,
                                 ILogger<LoginController> logger) : ControllerBase
    {
        private readonly IUserManager userManager = userManager;
        private readonly ILogger<LoginController> logger = logger;

        private readonly IConfiguration configuration = configuration.GetSection(AuthorizationSetup.JWT_SETTINGS_SECTION_NAME);

        // POST: api/login
        [HttpPost("login")]
        public async Task Login([FromBody] CredentialsDTO credentials, CancellationToken ct)
        {
            new CredentialsValidator().Validate(credentials);

            logger.LogInformation($"Trying to log in as: {credentials.UserName}");
            if (!await userManager.AuthenticateUser(credentials.UserName!, credentials.Password!, ct))
                throw new HttpCredentialsException("Invalid username or passphrase");

            logger.LogInformation($"{credentials.UserName} succesfully logged in");
            AddJwtTokenToCookies(credentials.UserName!);
        }

        // POST: api/register
        [HttpPost("register")]
        public async Task Register([FromBody] CredentialsDTO credentials, CancellationToken ct)
        {
            new CredentialsValidator().Validate(credentials);

            logger.LogInformation($"Trying to register new user: {credentials.UserName}");
            await userManager.CreateUser(credentials.UserName!, credentials.Password!, ct);

            logger.LogInformation($"{credentials.UserName} succesfully registered");
            AddJwtTokenToCookies(credentials.UserName!);
        }

        private void AddJwtTokenToCookies(string user)
        {
            var token = CreateJWTToken(user);
            Response.Cookies.Append(AuthorizationSetup.AUTHORIZATION_COOKIE, token);
        }

        private string CreateJWTToken(string user)
        {
            var signinCredentials = GenerateSigningCredentials();
            var claims = new List<Claim> { new (ClaimTypes.Name, user)};

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: configuration[AuthorizationSetup.ISSUER_CONFIG],
                audience: configuration[AuthorizationSetup.AUDIENCE_CONFIG],
                // Long expire time not advisible for production, but removes the need for token refreshing
                expires: DateTime.Now.AddHours(8),
                signingCredentials: signinCredentials,
                claims: claims
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private SigningCredentials GenerateSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(configuration[AuthorizationSetup.SECRET_KEY_CONFIG]!);
            return new SigningCredentials(new SymmetricSecurityKey(key),
                                          SecurityAlgorithms.HmacSha256);
        }
    }
}
