using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebHooks.Filters;
using Microsoft.AspNetCore.WebHooks.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.WebHooks.Internal
{
    public static class SnsServiceCollectionSetup
    {
        public static void AddSnsServices(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcOptionsSetup>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IWebHookMetadata, SnsMetadata>());
            services.TryAddSingleton<SnsVerifySignatureFilter>();
        }

        private class MvcOptionsSetup : IConfigureOptions<MvcOptions>
        {
            public void Configure(MvcOptions options)
            {
                if (options == null)
                    throw new ArgumentNullException(nameof(options));

                options.Filters.AddService<SnsVerifySignatureFilter>(WebHookSecurityFilter.Order);
            }
        }
    }
}
