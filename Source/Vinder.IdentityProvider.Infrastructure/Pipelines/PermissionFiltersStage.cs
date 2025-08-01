namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class PermissionFiltersStage
{
    public static PipelineDefinition<Permission, BsonDocument> FilterPermissions(
        this PipelineDefinition<Permission, BsonDocument> pipeline,
        PermissionFilters filters)
    {
        var specifications = BuildMatchFilter(filters);
        return pipeline.Match(specifications);
    }

    private static FilterDefinition<BsonDocument> BuildMatchFilter(PermissionFilters filters)
    {
        var filterDefinitions = new List<FilterDefinition<BsonDocument>>
        {
            MatchIfNotEmpty(DocumentFields.Permission.Name, filters.Name),
            MatchIfNotEmptyGuid(DocumentFields.Permission.TenantId, filters.TenantId)
        };

        if (!filters.IsDeleted.HasValue)
        {
            filterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(DocumentFields.Permission.IsDeleted, false));
        }
        else
        {
            filterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(DocumentFields.Permission.IsDeleted, filters.IsDeleted.Value));
        }

        return Builders<BsonDocument>.Filter.And(filterDefinitions);
    }

    private static FilterDefinition<BsonDocument> MatchIfNotEmpty(string field, string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? FilterDefinition<BsonDocument>.Empty
            : Builders<BsonDocument>.Filter.Eq(field, BsonValue.Create(value));
    }

    private static FilterDefinition<BsonDocument> MatchIfNotEmptyGuid(string field, Guid? value)
    {
        return !value.HasValue || value == Guid.Empty
            ? FilterDefinition<BsonDocument>.Empty
            : Builders<BsonDocument>.Filter.Eq(field, new BsonBinaryData(value.Value, GuidRepresentation.Standard));
    }
}
