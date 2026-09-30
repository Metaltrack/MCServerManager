using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace MCServerManager.MCServerManager.Core.Services;

public class NotificationConfig
{
    public string WebhookUrl { get; set; } = string.Empty;
}

public class NotificationService
{
    private string webhook = string.Empty;
    
    public NotificationService()
    {
        string configFile = "D:\\Visual Studio Projects\\MinecraftServerManager\\MCServerManager\\config.json";
        string jsonData = File.ReadAllText(configFile);
        NotificationConfig? config = JsonSerializer.Deserialize<NotificationConfig>(jsonData);
        webhook = config?.WebhookUrl ?? string.Empty;
        Console.WriteLine($"\nWebHook: {webhook}\n");
    }
    public async Task<string> SendNotification(string message)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "curl.exe",

            RedirectStandardOutput = true,
            RedirectStandardError = true,

            UseShellExecute = false,
            CreateNoWindow = true
        };

        startInfo.ArgumentList.Add(
                $"-X"
            );

        startInfo.ArgumentList.Add(
                $"POST"
            );

        startInfo.ArgumentList.Add(
                $"{webhook}"
            );

        startInfo.ArgumentList.Add(
                $"-H"
            );

        startInfo.ArgumentList.Add(
                $"Content-Type: application/json"
            );

        startInfo.ArgumentList.Add(
                $"-d"
            );

        startInfo.ArgumentList.Add(
                $"{{\"content\": \"{message}\"}}"
            );

        Console.WriteLine("Running curl with following args: \n-------------------------------");
        foreach(string arg in startInfo.ArgumentList)
        {
            Console.WriteLine(arg);
        }
        Console.WriteLine("-------------------------------\n");

        using var process = new Process { StartInfo = startInfo };
        process.Start();
        process.WaitForExit();
        return process.StandardError.ReadToEnd();
    }
}
