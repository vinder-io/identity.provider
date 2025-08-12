namespace Vinder.IdentityProvider.Infrastructure.Repositories;

public sealed class TokenRepository(IMongoDatabase database, ITenantProvider tenantProvider) :
    BaseRepository<SecurityToken>(database, Collections.Tokens),
    ITokenRepository
{
    public async Task<IReadOnlyCollection<SecurityToken>> GetTokensAsync(TokenFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<SecurityToken>()
            .As<SecurityToken, SecurityToken, BsonDocument>()
            .FilterTokens(filters, tenantProvider)
            .Paginate(filters);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var tokens = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<SecurityToken>(bson))
            .ToList();

        return tokens;
    }

    public async Task<long> CountAsync(TokenFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<SecurityToken>()
            .As<SecurityToken, SecurityToken, BsonDocument>()
            .FilterTokens(filters, tenantProvider)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        if (result == null)
        {
            return 0;
        }

        return result.Count;
    }
}