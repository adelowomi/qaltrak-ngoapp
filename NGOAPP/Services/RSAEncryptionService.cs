using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace NGOAPP;

public class RSAEncryptionService : IRSAEncryptionService
{
    private readonly ILogger<RSAEncryptionService> _logger;

    public RSAEncryptionService(ILogger<RSAEncryptionService> logger)
    {
        _logger = logger;
    }

    public async Task<string> EncryptAsync<T>(T data, string key)
    {
        string methodName = nameof(EncryptAsync);
        try
        {
            string encryptedString = string.Empty;

            var plainText = Regex.Unescape(JsonConvert.SerializeObject(data));

            plainText = plainText.Replace("\\", "");

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(plainText));
            var msRSA = RSA.Create();
            msRSA.ImportFromPem(key);

            int keySize = msRSA.KeySize / 8;
            int maxLength = keySize - 42;
            int dataLength = bytes.Length;
            int iterations = dataLength / maxLength;
            var finalBytes = new byte[0];
            List<byte> encryptedBytes = new List<byte>();

            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                byte[] encryptedChunk = msRSA.Encrypt(tempBytes, RSAEncryptionPadding.Pkcs1);
                encryptedBytes.AddRange(encryptedChunk);
            }
            finalBytes = encryptedBytes.ToArray();

            _logger.LogInformation("{methodName} After encryption  {@request} {encryptedBytes}", methodName, data, encryptedBytes);

            encryptedString = Convert.ToBase64String(finalBytes);
            return encryptedString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{methodName} Error encrypting request {@request}", methodName, data);
            throw;
        }
    }

    public async Task<T> DecryptAsync<T>(string response, string key)
    {
        string methodName = nameof(DecryptAsync);
        try
        {
            var msRSA = RSA.Create();

            _logger.LogInformation("{methodName} {encryptedString} {Privatekey}", methodName, response, key);

            msRSA.ImportFromPem(key);

            var decryptedBytes = msRSA.Decrypt(Convert.FromBase64String(response), RSAEncryptionPadding.Pkcs1);

            var decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            _logger.LogInformation("{methodName} {encryptedString} {decryptedString}", methodName, response, decryptedString);

            return JsonConvert.DeserializeObject<T>(decryptedString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{methodName} Error decrypting response {response}", methodName, response);
            throw;
        }
    }
}

public interface IRSAEncryptionService
{
    Task<string> EncryptAsync<T>(T data, string key);
    Task<T> DecryptAsync<T>(string response, string key);
}
