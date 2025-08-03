using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Vinder.IdentityProvider.Infrastructure.Security;

public sealed class ClaimsBuilder
{
    private readonly List<Claim> _claims = [];
    public IEnumerable<Claim> Build() => _claims;

    public ClaimsBuilder WithSubject(string subject)
    {
        _claims.Add(new Claim(JwtRegisteredClaimNames.Sub, subject));
        return this;
    }

    public ClaimsBuilder WithUsername(string username)
    {
        _claims.Add(new Claim(JwtRegisteredClaimNames.PreferredUsername, username));
        return this;
    }

    public ClaimsBuilder WithPermissions(IEnumerable<Permission> permissions)
    {
        foreach (var permission in permissions)
        {
            _claims.Add(new Claim(ClaimTypes.Role, permission.Name));
        }

        return this;
    }
}
