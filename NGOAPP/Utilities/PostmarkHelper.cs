using Microsoft.Extensions.Options;
using PostmarkDotNet;

namespace NGOAPP;

public class PostmarkHelper : IPostmarkHelper
{

    private readonly AppSettings _appSettings;

    public PostmarkHelper(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public async Task SendTemplatedEmail(string templateId, string to, Dictionary<string, string> templateModel)
    {
        var client = new PostmarkClient(_appSettings.PostMarkToken);
        var message = new TemplatedPostmarkMessage
        {
            From = _appSettings.PostMarkFromEmail,
            TemplateId = Convert.ToInt64(templateId),
            To = to,
            TemplateModel = templateModel
        };

        var result = await client.SendMessageAsync(message);
        if (result.Status != PostmarkStatus.Success)
        {
            throw new Exception("Failed to send email");
        }
    }
}

public interface IPostmarkHelper
{
    Task SendTemplatedEmail(string templateId, string to, Dictionary<string, string> templateModel);
}