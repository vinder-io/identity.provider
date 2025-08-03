namespace Vinder.IdentityProvider.Infrastructure.Repositories;

public sealed class TenantRepository(IMongoDatabase database) :
    BaseRepository<Tenant>(database, Collections.Tenants),
    ITenantRepository
{
    public async Task<IReadOnlyCollection<Tenant>> GetTenantsAsync(TenantFilters filters, CancellationToken cancellation)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Tenant>()
            .As<Tenant, Tenant, BsonDocument>()
            .FilterTenants(filters)
            .Paginate(filters);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var tenants = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Tenant>(bson))
            .ToList();

        return tenants;
    }

    public async Task<long> CountAsync(TenantFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Tenant>()
            .As<Tenant, Tenant, BsonDocument>()
            .FilterTenants(filters)
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
