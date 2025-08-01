namespace Vinder.IdentityProvider.Domain.Repositories;

public interface ITokenRepository : IRepository<SecurityToken>
{
    public Task<IReadOnlyCollection<SecurityToken>> GetTokensAsync(
        TokenFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountAsync(
        TokenFilters filters,
        CancellationToken cancellation = default
    );
}