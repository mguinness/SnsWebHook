using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.WebHooks.Metadata
{
    public class SnsMetadata : WebHookMetadata, IWebHookBodyTypeMetadataService, IWebHookGetHeadRequestMetadata
    {
        public SnsMetadata() : base(SnsConstants.ReceiverName)
        {
        }

        public override WebHookBodyType BodyType => WebHookBodyType.Json;

        public bool AllowHeadRequests => false;

        public string ChallengeQueryParameterName => null;

        public int SecretKeyMinLength => SnsConstants.SecretKeyMinLength;

        public int SecretKeyMaxLength => SnsConstants.SecretKeyMaxLength;
    }
}
