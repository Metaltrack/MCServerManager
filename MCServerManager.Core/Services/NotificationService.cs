using MCServerManager.MCServerManager.Core.Models;
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

public class Payload
{
    public string message = string.Empty;
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
                $"{{\"content\": {message}}}"
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

    public async Task<string> ServerStart(ServerProfile profile, UserModel user)
    {
        string message =$"```ansi\r\n\u001b[0;37m██▀▀█▀▀█ ██ ██▀▀█ ██ ██▀▀▀▀▀▀ ██▀▀▀▀▀▀ ██▀▀▀▀▀█ █▀▀▀▀▀█ ██▀▀▀▀▀▀ ▀▀▀▀█▀▀▀\u001b[0m\r\n\u001b[0;37m██ ██ ██ ██ ██ ██ ██ ██▄▄▄▄▄  ██       ██▄▄▄▄▄█ █▄▄▄▄▄█ ██▄▄▄▄▄     ██   \u001b[0m\r\n\u001b[0;37m██    ██ ██ ██ ██ ██ ██       ██       ██   ▄█  █    ▄█ ██          ██   \u001b[0m\r\n\u001b[0;37m██    ██ ██ ██ ██▄▄█ ██▄▄▄▄▄▄ ██▄▄▄▄▄▄ ██   ██▄ █     █ ██          ██   \u001b[0m\r\n```" +

            $"\n\n----------------------------------------------------------------\n\n" +
            $"=ON= Server {profile.Name} | is active\n" +
            $"At port {profile.Port}\n" +
            $"User: {user.UserName}\n" +
            $"Connection: {user.TailScaleIP}:{profile.Port}\n" +
            $"User Message: {user.UserMessage}" +
            $"\n\n----------------------------------------------------------------\n";

        Payload payload = new Payload();
        payload.message = message;

        string msg = JsonSerializer.Serialize(payload.message);

        return await SendNotification(msg);
    }

    public async Task<string> ServerStop(ServerProfile profile, UserModel user)
    {
        string message = $"```ansi\r\n\u001b[0;37m██▀▀█▀▀█ ██ ██▀▀█ ██ ██▀▀▀▀▀▀ ██▀▀▀▀▀▀ ██▀▀▀▀▀█ █▀▀▀▀▀█ ██▀▀▀▀▀▀ ▀▀▀▀█▀▀▀\u001b[0m\r\n\u001b[0;37m██ ██ ██ ██ ██ ██ ██ ██▄▄▄▄▄  ██       ██▄▄▄▄▄█ █▄▄▄▄▄█ ██▄▄▄▄▄     ██   \u001b[0m\r\n\u001b[0;37m██    ██ ██ ██ ██ ██ ██       ██       ██   ▄█  █    ▄█ ██          ██   \u001b[0m\r\n\u001b[0;37m██    ██ ██ ██ ██▄▄█ ██▄▄▄▄▄▄ ██▄▄▄▄▄▄ ██   ██▄ █     █ ██          ██   \u001b[0m\r\n```" +

            $"\n\n----------------------------------------------------------------\n\n" +
            $"-OFF- Server {profile.Name} | is inactive\n" +
            $"User: {user.UserName}\n" +
            $"Connection: Activate server to view connection!\n" +
            $"User Message: User has shutdown the server" +
            $"\n\n----------------------------------------------------------------\n";

        Payload payload = new Payload();
        payload.message = message;

        string msg = JsonSerializer.Serialize(payload.message);

        return await SendNotification(msg);
    }
}
