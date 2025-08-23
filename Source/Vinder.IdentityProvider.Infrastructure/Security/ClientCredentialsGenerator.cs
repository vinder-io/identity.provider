namespace Vinder.IdentityProvider.Infrastructure.Security;

public sealed class ClientCredentialsGenerator(IPasswordHasher passwordHasher) : IClientCredentialsGenerator
{
    public async Task<(string clientId, string clientSecret)> GenerateAsync(string tenantName)
    {
        var bytes = new byte[32];

        RandomNumberGenerator.Fill(bytes);

        var clientId = Convert.ToHexString(bytes).ToLowerInvariant();
        var clientSecret = await passwordHasher.HashPasswordAsync(clientId + tenantName);

        return (clientId, clientSecret);
    }
}