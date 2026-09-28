using MCServerManager.MCServerManager.Core.Models;
using System.Diagnostics;

namespace MCServerManager.MCServerManager.Core.Services;

public class GitService{
    private readonly string _repoPath;

    public GitService(string repoPath)
    {
        _repoPath = repoPath;
    }

    public async Task<bool> CheckGit()
    {
        var result = await RunGitAsync("--version");
        if (!result.Success)
        {
            Console.WriteLine("'git' does not exists, you will have to install git!");
        }
        else
        {
            Console.WriteLine("git check complete. git found!");
            return true;
        }
        return false;
    }

    public async Task<GitResult> RunGitAsync(params string[] arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "git",
            WorkingDirectory = _repoPath,

            RedirectStandardOutput = true,
            RedirectStandardError = true,

            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach(string arg in arguments)
        {
            startInfo.ArgumentList.Add(arg);
        }

        using var process = new Process { StartInfo = startInfo };

        process.Start();

        Task<string> TaskOutput = process.StandardOutput.ReadToEndAsync();
        Task<string> TaskError = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        return new GitResult
        {
            ExitCode = process.ExitCode,
            Output = await TaskOutput,
            Error = await TaskError
        };
    }

    public Task<GitResult> GetStatusAsync()
    {
        return RunGitAsync("status", "--short");
    }

    public Task<GitResult> PullAsync()
    {
        return RunGitAsync("pull");
    }

    public Task<GitResult> AddAllAsync()
    {
        return RunGitAsync(
            "add",
            "-A"
        );
    }

    public Task<GitResult> CommitAsync(string message)
    {
        return RunGitAsync(
            "commit",
            "-m",
            message
        );
    }

    public Task<GitResult> PushAsync()
    {
        return RunGitAsync("push");
    }
}
