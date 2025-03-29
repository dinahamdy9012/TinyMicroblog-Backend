using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TinyMicroblog.Shared.Infrastructure.Security;

public class CurrentUserService(ITokenService tokenService, ICookieService cookieService) : ICurrentUserService
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly ICookieService _cookieService = cookieService;

    public (int userId,string username) GetCurrentUser()
    {
        var jwtCookie = _cookieService.GetToken();
        var token = _tokenService.ValidateToken(jwtCookie);

        var userId = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value;
        var username = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
        return (int.Parse(userId), username);

    }
    public string GetCurrentUserCountry()
    {
        var jwtCookie = _cookieService.GetToken();
        var token = _tokenService.ValidateToken(jwtCookie);

        return token.Claims.First(x => x.Type == ClaimTypes.Country).Value;
    }
}
