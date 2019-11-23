using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.WebHooks
{
    public class SnsConstants
    {
        public static string ReceiverName => "sns";
        public static int SecretKeyMinLength => 1;
        public static int SecretKeyMaxLength => 100;
        public static string EventPropertyName = "x-amz-sns-message-type";
        public static string IdPropertyName = "x-amz-sns-topic-arn";
    }
}
