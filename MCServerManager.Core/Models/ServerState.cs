namespace MCServerManager.MCServerManager.Core.Models;

public enum ServerState
{
    Initializing,

    Ready,

    Pulling,

    Starting,

    Running,

    Stopping,

    Reviewing,

    Committing,

    ReadyToPush,

    Pushing,

    Error
}