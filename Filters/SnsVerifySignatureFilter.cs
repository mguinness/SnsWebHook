using Amazon.SimpleNotificationService.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.WebHooks.Filters
{
    public class SnsVerifySignatureFilter : WebHookVerifySignatureFilter, IAsyncResourceFilter
    {
        private readonly IWebHookRequestReader _requestReader;

        public SnsVerifySignatureFilter(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory, IWebHookRequestReader requestReader)
            : base(configuration, hostingEnvironment, loggerFactory)
        {
            _requestReader = requestReader;
        }

        public override string ReceiverName => SnsConstants.ReceiverName;

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (next == null)
                throw new ArgumentNullException(nameof(next));

            var routeData = context.RouteData;
            var request = context.HttpContext.Request;

            if (routeData.TryGetWebHookReceiverName(out var receiverName) && IsApplicable(receiverName) && HttpMethods.IsPost(request.Method))
            {
                var errorResult = EnsureSecureConnection(ReceiverName, context.HttpContext.Request);
                if (errorResult != null)
                {
                    context.Result = errorResult;
                    return;
                }
            }
            else
            {
                await next();
                return;
            }

            var eventName = GetRequestHeader(request, SnsConstants.EventPropertyName, out IActionResult errorResultMsgType);
            if (errorResultMsgType != null)
            {
                context.Result = errorResultMsgType;
                return;
            }

            routeData.Values[WebHookConstants.EventKeyName] = eventName;

            var idName = GetRequestHeader(request, SnsConstants.IdPropertyName, out IActionResult errorResultTopicArn);
            if (errorResultTopicArn != null)
            {
                context.Result = errorResultTopicArn;
                return;
            }

            routeData.Values[WebHookConstants.IdKeyName] = idName;

            /*var secretKey = GetSecretKey(ReceiverName, routeData, SnsConstants.SecretKeyMinLength, SnsConstants.SecretKeyMaxLength);
            if (secretKey == null)
            {
                context.Result = new NotFoundResult();
                return;
            }*/

            //https://forums.aws.amazon.com/message.jspa?messageID=261061#261061
            request.ContentType = MediaTypeNames.Application.Json;

            var data = await _requestReader.ReadBodyAsync<JObject>(context);
            var snsMsg = Message.ParseMessage(data.ToString());

            //Base64-encoded "SHA1withRSA" signature of the Message, MessageId, Subject (if present), Type, Timestamp, and TopicArn values
            if (!snsMsg.IsMessageSignatureValid())
            {
                context.Result = new BadRequestResult();
                return;
            }

            await next();
        }
    }
}
