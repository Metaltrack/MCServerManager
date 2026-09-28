using MCServerManager.MCServerManager.Core.Models;
using MCServerManager.MCServerManager.Core.Services;

namespace MCServerManager.MCServerManager.Core;

public class ServerManager
{
    private readonly GitService _git;
    private readonly JavaHandlerService _java;
    private readonly MinecraftServerService _minecraft;

    public ServerProfile Profile { get; }

    public ServerState State { get; private set; }
        = ServerState.Initializing;

    public event Action<ServerState>? StateChanged;

    public event Action<string>? LogReceived;

    public ServerManager(ServerProfile profile)
    {
        Profile = profile;

        _git = new GitService(profile.RepositoryPath);

        _java = new JavaHandlerService(
            profile.RepositoryPath
        );

        _minecraft = new MinecraftServerService(profile);

        _minecraft.OutputReceived += message =>
        {
            LogReceived?.Invoke(message);
        };
    }
}