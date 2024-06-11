using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NGOAPP;

[ApiController]
[Route("api/[controller]")]
public class TesterController : StandardControllerBase
{
    IRSAEncryptionService _rsaEncryptionService;
    private readonly AppSettings _config;
    private readonly ILogger<TesterController> _logger;

    public TesterController(IRSAEncryptionService rsaEncryptionService, IOptions<AppSettings> config, ILogger<TesterController> logger)
    {
        _rsaEncryptionService = rsaEncryptionService;
        _config = config.Value;
        _logger = logger;
    }

    public async Task<string> EncryptRequestAsync<T>(T request, string sessionId, string tempCertPath = null)
    {
        const string methodName = nameof(EncryptRequestAsync);

        string encryptedString = string.Empty;

        try
        {
            encryptedString = await _rsaEncryptionService.EncryptAsync(request, _config.EncryptionKey);

            return encryptedString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{methodName} {@request} {sessionId}", methodName, request, sessionId);
        }

        return encryptedString;
    }

    public async Task<T> DecryptResponse<T>(string response)
    {
        const string methodName = nameof(DecryptResponse);
        try
        {
            var decryptedResponse = await _rsaEncryptionService.DecryptAsync<T>(response, _config.DecryptionKey);
            return decryptedResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{methodName} {encryptedString}", methodName, response);
            throw;
        }
    }

    [HttpPost("pp/encrypt", Name = nameof(TestEncyptionWithGfPublicKey))]
    public async Task<ActionResult> TestEncyptionWithGfPublicKey([FromBody] DecryptRequestDto dataToEncrypt)
    {
        var res = await EncryptRequestAsync(dataToEncrypt.StringToEncrypt, "1");
        return Ok(new { EncryptedText = res });
    }

    [HttpPost("pp/decrypt", Name = nameof(TestDecryptionWithGfPrivateKey))]
    public async Task<ActionResult> TestDecryptionWithGfPrivateKey([FromBody] DecryptRequestDto data)
    {
        return Ok(await DecryptResponse<string>(data.EncryptedText));
    }
}

public class DecryptRequestDto
{
    public string EncryptedText { get; set; }
    public string StringToEncrypt { get; set; }
}
