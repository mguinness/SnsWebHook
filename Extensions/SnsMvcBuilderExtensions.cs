using Microsoft.AspNetCore.WebHooks.Internal;
using Microsoft.AspNetCore.WebHooks.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SnsMvcBuilderExtensions
    {
        public static IMvcBuilder AddSnsWebHooks(this IMvcBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            WebHookMetadata.Register<SnsMetadata>(builder.Services);

            SnsServiceCollectionSetup.AddSnsServices(builder.Services);

            return builder.AddWebHooks();
        }
    }
}
