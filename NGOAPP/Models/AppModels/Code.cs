using System;

namespace NGOAPP.Models.AppModels;

public class Code : BaseModel
{
    public string CodeString { get; set; }
    public string Description { get; set; }
    public string? Placeholder { get; set; }
}
