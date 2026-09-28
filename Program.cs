using MCServerManager.MCServerManager.Core.Models;
using MCServerManager.MCServerManager.Core.Services;

var profile = new ServerProfile
{
    RepositoryPath = @"D:\Visual Studio Projects\MinecraftServerManager\TestMinecraftServer\TheTestServer",
    ServerJar = "server.jar",
    WorldName = "Test-World",
    Port = 4567,
    MinimumMemoryMb = 2048,
    MaximumMemoryMb = 4096,
    JavaExecutable = "java"
};

var minecraft =
    new MinecraftServerService(profile);

minecraft.OutputReceived += Console.WriteLine;

minecraft.ProcessExited += exitCode =>
{
    Console.WriteLine(
        $"Server exited with code {exitCode}"
    );
};

bool started =
    await minecraft.StartAsync();

if (!started)
{
    Console.WriteLine(
        "Could not start Minecraft server."
    );

    return;
}

while (minecraft.IsRunning)
{
    string? command =
        Console.ReadLine();

    if (string.IsNullOrWhiteSpace(command))
        continue;

    await minecraft.SendCommandAsync(command);

    if (command.Equals(
        "stop",
        StringComparison.OrdinalIgnoreCase))
    {
        await minecraft.WaitForExitAsync();

        break;
    }
}