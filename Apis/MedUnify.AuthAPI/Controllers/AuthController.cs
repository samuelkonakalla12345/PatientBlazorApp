namespace MedUnify.AuthAPI.Controllers
{
    using global::MedUnify.AuthAPI.Services.Interface;
    using global::MedUnify.ResourceModel;
    using global::MedUnify.ResourceModel.Auth;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.ComponentModel.DataAnnotations;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace MedUnify.AuthAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly IOAuthClientService _oAuthClientService;
            private readonly IConfiguration _configuration;

            public AuthController(IOAuthClientService oAuthClientService, IConfiguration configuration)
            {
                this._oAuthClientService = oAuthClientService;
                this._configuration = configuration;
            }

            [HttpPost]
            [AllowAnonymous]
            [Route("GetToken")]
            //[ServiceFilter(typeof(LoggingActionFilter))]
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseResourceModel))]
            [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationResult))]
            [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ReturnStatusResourseModel))]
            public async Task<IActionResult> Post(TokenRequestResourceModel model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                TokenResponseResourceModel authToken = new TokenResponseResourceModel();
                ReturnStatusResourseModel returnStatusRM = new ReturnStatusResourseModel();

                authToken.CreatedOn = DateTime.UtcNow;
                authToken.ExpireOn = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["Jwt:AuthTokenExpireInMinutes"]));

                var client = await this._oAuthClientService.GetClientByClientIdAsync(model.ClientId);

                if (client == null || client.ClientSecret != model.ClientSecret)
                {
                    returnStatusRM.Status = "warning";
                    returnStatusRM.Title = "Not Found!";
                    returnStatusRM.Message = "ClientId or ClientSecret is incorrect. Please try again.";
                    return this.Unauthorized(returnStatusRM);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, client.ClientId),
                    new Claim("OrganizationId", client.OrganizationId.ToString()),
                    new Claim("OrganizationName", client.OrganizationName.ToString()),
                    new Claim(ClaimTypes.Role, "User")
                    }),
                    Expires = authToken.ExpireOn,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["Jwt:ValidIssuer"],
                    Audience = _configuration["Jwt:ValidAudience"]
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                authToken.AuthToken = tokenString;

                return Ok(authToken);
            }
        }
    }
}