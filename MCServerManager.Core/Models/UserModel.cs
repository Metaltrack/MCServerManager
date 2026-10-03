using System;
using System.Collections.Generic;
using System.Text;

namespace MCServerManager.MCServerManager.Core.Models;

public class UserModel
{
    public string UserName { get; set; } = string.Empty;
    public string TailScaleIP { get; set; } = string.Empty;

    public string UserMessage { get; set; } = string.Empty;
}
