using System;
using System.Collections.Generic;
using System.Text;

namespace MCServerManager.MCServerManager.Core.Models;

public class ServerProfile
{
    public string Name { get; set; } = "Minecraft Server";

    public string RepositoryUrl { get; set; } = string.Empty;

    public string RepositoryPath { get; set; } = string.Empty;

    public string ServerJar { get; set; } = "server.jar";

    public string WorldName { get; set; } = "world";

    public int Port { get; set; } = 25565;

    public int MinimumMemoryMb { get; set; } = 2048;

    public int MaximumMemoryMb { get; set; } = 4096;

    public string JavaExecutable { get; set; } = "java";

    public string Branch { get; set; } = "main";
}
