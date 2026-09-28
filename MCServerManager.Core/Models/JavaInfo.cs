using System;
using System.Collections.Generic;
using System.Text;

namespace MCServerManager.MCServerManager.Core.Models;

public class JavaInfo
{
    public bool IsAvailable { get; set; }
    public string VersionString { get; set; } = string.Empty;
    public string MajorVersion { get; set; } = string.Empty;
}
