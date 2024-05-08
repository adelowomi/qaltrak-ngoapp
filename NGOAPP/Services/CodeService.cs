using System;
using System.Linq;
using NGOAPP.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace NGOAPP.Services;

public class CodeService : ICodeService
{
    private readonly IBaseRepository<Code> _codeRepository;

    public CodeService(IBaseRepository<Code> codeRepository)
    {
        _codeRepository = codeRepository;
    }

    /// <summary>
    /// Generate a random code that is not in the database with the given length
    /// </summary>
    /// <param name="length">The length of the code to generate</param>
    /// <param name="prefix">The prefix to use on the code</param>
    /// <param name="suffix">The suffix to use on the code</param> 
    /// <returns>code</returns>
    public string GenerateCode(string description, int length, string prefix = "", string suffix = "", string placeholder = null, bool numberOnly = false)
    {
        var code = string.Empty;
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        if (numberOnly)
        {
            chars = "0123456789";
        }
        var charLength = chars.Length;
        var codeExists = true;

        while (codeExists)
        {
            code = prefix;
            for (var i = 0; i < length; i++)
            {
                code += chars[random.Next(charLength)];
            }
            code += suffix;

            codeExists = _codeRepository.Any(c => c.CodeString == code);
        }
        // create an entry in the database for the code
        _codeRepository.Create(new Code { CodeString = code, Description = description, Placeholder = placeholder });
        return code;
    }

    /// <summary>
    /// Get a code from the database
    /// </summary>
    /// <param name="code">The code to get</param>
    /// <returns>code</returns>
    public Code GetCode(string code)
    {
        return _codeRepository.Query().FirstOrDefault(c => c.CodeString == code);
    }

    /// <summary>
    /// destroy a code from the database by id
    /// </summary>
    /// <param name="id">The id of the code to destroy</param>
    /// <returns></returns>
    public void DestroyCode(int id)
    {
        var code = _codeRepository.GetById(id);
        _codeRepository.Delete(code);
    }
}

public interface ICodeService
{
    string GenerateCode(string description, int length, string prefix = "", string suffix = "", string placeholder = null, bool numberOnly = false);
    Code GetCode(string code);
    void DestroyCode(int id);
}
