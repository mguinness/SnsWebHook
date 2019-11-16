# Webhook support for Amazon SNS in ASP.NET Core

Accept inbound Amazon SNS notification messages.

# Configuring Project

1. Add [SnsWebHook](https://www.nuget.org/packages/SnsWebHook) NuGet package into your project
2. Modify your `ConfigureServices` method in `Startup.cs` file
```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
      .AddSnsWebHooks();
}
```
3. Add Controller class with `[SnsWebHook]` attribute

```CSharp
public class SnsController : ControllerBase
{
    [SnsWebHook]
    public async Task<IActionResult> SnsHandler(string @event, JObject data)
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine(data.Value<string>("Message"));

            if (@event == "SubscriptionConfirmation")
            {
                var url = data.Value<string>("SubscribeURL");
                var response = await new HttpClient().GetAsync(url);
            }
            else
            {
                Console.WriteLine("Type {0} with payload {1}", @event, data);
            }

            return NoContent();
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}
```
# Configuring Webhook

See instructions for [Getting Started with Amazon SNS](https://docs.aws.amazon.com/sns/latest/dg/sns-getting-started.html) on Amazon.

The callback address will be https://server/api/webhooks/incoming/sns and you'll need to replace server with your host.