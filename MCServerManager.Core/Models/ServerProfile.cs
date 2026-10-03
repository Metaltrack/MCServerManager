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

    public List<string> JavaArgs { get; set; } = new()
    {
        "-XX:+UseG1GC",
        "-XX:+ParallelRefProcEnabled",
        "-XX:MaxGCPauseMillis=200",
        "-XX:+UnlockExperimentalVMOptions",
        "-XX:+DisableExplicitGC",
        "-XX:+AlwaysPreTouch",
        "-XX:G1NewSizePercent=30",
        "-XX:G1MaxNewSizePercent=40",
        "-XX:G1HeapRegionSize=8M",
        "-XX:G1ReservePercent=20",
        "-XX:G1HeapWastePercent=5",
        "-XX:G1MixedGCCountTarget=4",
        "-XX:InitiatingHeapOccupancyPercent=15",
        "-XX:G1MixedGCLiveThresholdPercent=90",
        "-XX:G1RSetUpdatingPauseTimePercent=5",
        "-XX:SurvivorRatio=32",
        "-XX:+PerfDisableSharedMem",
        "-XX:MaxTenuringThreshold=1",
        "-Dusing.aikars.flags=https://mcflags.emc.gs",
        "-Daikars.new.flags=true"
    };
}
