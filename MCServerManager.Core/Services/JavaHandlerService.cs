using MCServerManager.MCServerManager.Core.Models;
using System.Diagnostics;

namespace MCServerManager.MCServerManager.Core.Services;

public class JavaHandlerService
{
    private readonly string _repoPath;
    private Process? _process;
    public bool IsRunning => _process is not null && !_process.HasExited;

    public JavaInfo Info;

    public JavaHandlerService(string repoPath)
    {
        _repoPath = repoPath;
        Info = new JavaInfo();
    }

    public async Task<string?> RunJavaAsync(params string[] arguments)
    {
        if (IsRunning)
        {
            return null;
        }

        if (string.IsNullOrEmpty(Info.ExecutablePath))
        {
            Info.ExecutablePath = "java";
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = Info.ExecutablePath,
            WorkingDirectory = _repoPath,

            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,

            UseShellExecute = false,
            CreateNoWindow = true
        };
        Console.WriteLine($"stuff: {startInfo.FileName}");
        foreach (string argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        _process = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };

        try
        {
            _process.Start();
        }
        catch
        {
            return null;
        }

        Task<string> outputTask =
            _process.StandardOutput.ReadToEndAsync();

        Task<string> errorTask =
            _process.StandardError.ReadToEndAsync();

        await _process.WaitForExitAsync();

        string output = await outputTask;
        string error = await errorTask;

        if (!string.IsNullOrWhiteSpace(output))
            return output;

        if (!string.IsNullOrWhiteSpace(error))
            return error;

        return null;
    }

    public async Task CheckJavaRuntimeAsync()
    {
        string? data = await RunJavaAsync("-version");

        if (string.IsNullOrWhiteSpace(data))
        {
            Console.WriteLine("Java runtime not found.");
            return;
        }

        Console.WriteLine("Java runtime detected:");
        Console.WriteLine(data);

        string? majorVersion = ParseMajorVersion(data);

        if (majorVersion is null)
        {
            Console.WriteLine("Could not parse Java version.");
            return;
        }

        Console.WriteLine($"Java major version: {majorVersion}");
    }

    private string? ParseMajorVersion(string data)
    {
        string[] sections = data.Split('"');

        if (sections.Length < 2)
            return null;

        string version = sections[1];

        if (version.StartsWith("1."))
        {
            string[] parts = version.Split('.');

            if (parts.Length < 2)
                return null;

            return parts[1];
        }

        string[] modernVersionParts = version.Split('.');

        Info = new JavaInfo
        {
            MajorVersion = modernVersionParts[0],
            VersionString = version,
            IsAvailable = true
        };

        return modernVersionParts[0];
    }
}