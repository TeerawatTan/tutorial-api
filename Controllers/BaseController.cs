using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace tutorial_api_2.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
        protected string GetAccessTokenFromHeader()
        {
            string jwt = Request.Headers[HeaderNames.Authorization];

            string[] tokens = jwt.Split(' ');

            if (tokens[0].ToLower() != "bearer")
            {
                return "";
            }
            string accessTokenString = tokens[1];
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessTokenString);
            return token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
    }
}
