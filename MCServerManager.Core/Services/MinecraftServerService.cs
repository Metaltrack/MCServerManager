using MCServerManager.MCServerManager.Core.Models;
using System.Diagnostics;

namespace MCServerManager.MCServerManager.Core.Services;

public class MinecraftServerService
{
    private readonly ServerProfile _profile;
    private Process? _process;

    public bool IsRunning =>
        _process is not null &&
        !_process.HasExited;

    public event Action<string>? OutputReceived;
    public event Action<int>? ProcessExited;

    public MinecraftServerService(ServerProfile profile)
    {
        _profile = profile;
    }

    public async Task<bool> StartAsync()
    {
        if (IsRunning)
            return false;

        var startInfo = new ProcessStartInfo
        {
            FileName = _profile.JavaExecutable,
            WorkingDirectory = _profile.RepositoryPath,

            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,

            UseShellExecute = false,
            CreateNoWindow = true
        };

        startInfo.ArgumentList.Add(
            $"-Xms{_profile.MinimumMemoryMb}M"
        );

        startInfo.ArgumentList.Add(
            $"-Xmx{_profile.MaximumMemoryMb}M"
        );

        foreach(string arg in _profile.JavaArgs)
        {
            startInfo.ArgumentList.Add(arg);
        }

        startInfo.ArgumentList.Add("-jar");
        startInfo.ArgumentList.Add(_profile.ServerJar);

        startInfo.ArgumentList.Add("--world");
        startInfo.ArgumentList.Add(_profile.WorldName);

        startInfo.ArgumentList.Add("--port");
        startInfo.ArgumentList.Add(
            _profile.Port.ToString()
        );

        //startInfo.ArgumentList.Add("nogui");

        _process = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };

        _process.OutputDataReceived += (_, args) =>
        {
            if (!string.IsNullOrWhiteSpace(args.Data))
            {
                OutputReceived?.Invoke(args.Data);
            }
        };

        _process.ErrorDataReceived += (_, args) =>
        {
            if (!string.IsNullOrWhiteSpace(args.Data))
            {
                OutputReceived?.Invoke(args.Data);
            }
        };

        _process.Exited += (_, _) =>
        {
            int exitCode = _process.ExitCode;

            ProcessExited?.Invoke(exitCode);
        };

        try
        {
            bool started = _process.Start();

            if (!started)
                return false;

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            await Task.CompletedTask;

            return true;
        }
        catch
        {
            _process?.Dispose();
            _process = null;

            return false;
        }
    }

    public async Task SendCommandAsync(string command)
    {
        if (!IsRunning || _process is null)
            return;

        await _process.StandardInput.WriteLineAsync(command);

        await _process.StandardInput.FlushAsync();
    }

    public async Task StopAsync()
    {
        if (!IsRunning || _process is null)
            return;

        await SendCommandAsync("stop");

        await _process.WaitForExitAsync();

        _process.Dispose();
        _process = null;
    }

    public async Task WaitForExitAsync()
    {
        if (_process is null)
            return;

        await _process.WaitForExitAsync();
    }
}