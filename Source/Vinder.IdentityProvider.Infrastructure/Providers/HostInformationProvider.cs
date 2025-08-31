using Microsoft.AspNetCore.Http;

namespace Vinder.IdentityProvider.Infrastructure.Providers;

public sealed class HostInformationProvider(IHttpContextAccessor accessor) : IHostInformationProvider
{
    public Uri Address
    {
        get
        {
            var request = accessor.HttpContext.Request;
            if (request is null)
            {
                throw new InvalidOperationException($"{nameof(HttpContext)} is not available.");
            }

            return new Uri($"{request.Scheme}://{request.Host}");
        }
    }
}