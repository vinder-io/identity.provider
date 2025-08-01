namespace Vinder.IdentityProvider.Domain.Filters.Builders;

public sealed class TokenFiltersBuilder
{
    private readonly TokenFilters _filters = new();

    public TokenFilters Build() => _filters;

    public TokenFiltersBuilder WithValue(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            _filters.Value = value;
        }

        return this;
    }

    public TokenFiltersBuilder WithType(TokenType? type)
    {
        if (type.HasValue)
        {
            _filters.Type = type;
        }

        return this;
    }

    public TokenFiltersBuilder WithUserId(Guid? userId)
    {
        if (userId.HasValue && userId != Guid.Empty)
        {
            _filters.UserId = userId;
        }

        return this;
    }

    public TokenFiltersBuilder WithTenantId(Guid? tenantId)
    {
        if (tenantId.HasValue && tenantId != Guid.Empty)
        {
            _filters.TenantId = tenantId;
        }

        return this;
    }

    public TokenFiltersBuilder WithPageNumber(int? pageNumber)
    {
        if (pageNumber.HasValue && pageNumber > 0)
        {
            _filters.PageNumber = pageNumber.Value;
        }

        return this;
    }

    public TokenFiltersBuilder WithPageSize(int? pageSize)
    {
        if (pageSize.HasValue && pageSize > 0)
        {
            _filters.PageSize = pageSize.Value;
        }

        return this;
    }
}
