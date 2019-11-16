using Microsoft.AspNetCore.WebHooks.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.WebHooks
{
    public class SnsWebHookAttribute : WebHookAttribute, IWebHookBodyTypeMetadata, IWebHookEventSelectorMetadata
    {
        public SnsWebHookAttribute() : base(SnsConstants.ReceiverName)
        {
        }

        public WebHookBodyType? BodyType => WebHookBodyType.Json;

        public string EventName { get; set; }
    }
}
