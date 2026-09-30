using MCServerManager.MCServerManager.Core.Models;
using MCServerManager.MCServerManager.Core.Services;
using System.Diagnostics.CodeAnalysis;

//First we start with running the minecraft server
/*
    Server file structure:
    
    GitFolder
        |
        |-- .git
        |
        |-- .gitattributes
        |-- .gitignore
        |
        |-- Server
        |
        |-- World
        |       |
        |       |-- World files
        |
        |-- Server.jar
        |
        |
        |-- Server stuff (more files)
 */

//We'll first need to provide java executable file, but we can first check automatically
//we also need the path to the server.jar

Console.WriteLine("Enter Path to git repository on your system: ");
var path_to_server = Console.ReadLine();
if (string.IsNullOrWhiteSpace(path_to_server))
{
    Console.WriteLine("Path to server is Invalid!");
    Environment.Exit(1);
}
else
{
    Console.WriteLine("Proceeding...\n");
}

var java = new JavaHandlerService(path_to_server);

//Auto check java version, and give ability to add separate version
/*
            ________________________
           |                        |
           |     Run Application    |
           |________________________|
                       |
                       |
                       V
            ________________________
           |                        |
           |      Check Java        |
           |________________________|
 */
await java.CheckJavaRuntimeAsync();
Console.Write($"--------------------------------\nJava Available: {java.Info.IsAvailable}\nJava Version: {java.Info.VersionString}\nJava Major Version: {java.Info.MajorVersion}\n" +
    $"--------------------------------");

Console.WriteLine("\nEnter the path to java executable (hit [Enter] to use default): ");
var executable_path = Console.ReadLine();
if (string.IsNullOrWhiteSpace(executable_path))
{
    Console.WriteLine("No path specified, using default path...");
    java.Info.ExecutablePath = "";
}
else
{
    Console.WriteLine($"Executable path: {executable_path}");
    java.Info.ExecutablePath = executable_path;
}

//Once Java is setup and good to go we need to get info on minecraft server
/*
    We need a minecraft server profile

    Server
        |
        |-- Server Name
        |
        |-- RepositoryURL
        |
        |-- RepositoryPath
        |
        |-- ServerJar Name (server.jar)
        |
        |-- WorldName (Exact name of the world folder in the server)
        |
        |-- Port (25565)
        |
        |-- minMemory
        |-- maxMemory
        |
        |-- JavaExecutable (Each server preset might use different java version)
        |
        |-- Branch (main)
 */

//This means now we need a minecraft server profile
//I have it hardcoded here, but it should be added through GUI later and saved aswell

var ServerProfile = new ServerProfile
{
    Name = "Test Server",
    RepositoryUrl = "https://github.com/Metaltrack/TestMinecraftServer.git",
    RepositoryPath = "D:\\Visual Studio Projects\\MinecraftServerManager\\TestMinecraftServer",
    ServerJar = "server.jar",
    WorldName = "Test-World",
    Port = 6767
};
//other values are defaults
//now we run the server

Console.Write($"--------------------------------\n" +
    $"Server Name: {ServerProfile.Name}\n" +
    $"Server RepoURL: {ServerProfile.RepositoryUrl}\n" +
    $"Server RepoPath: {ServerProfile.RepositoryPath}\n" +
    $"Server JarFile Name: {ServerProfile.ServerJar}\n" +
    $"Server World Name: {ServerProfile.WorldName}\n" +
    $"Server Port: {ServerProfile.Port}\n" +
    $"Server Minimum Memory Alloc: {ServerProfile.MinimumMemoryMb}Mb\n" +
    $"Server Maximum Memory Alloc: {ServerProfile.MaximumMemoryMb}Mb\n" +
    $"Server Java Executable: {ServerProfile.JavaExecutable}\n" +
    $"Server Repo Branch: {ServerProfile.Branch}\n" +
    $"--------------------------------\n");

Console.WriteLine("\nStarting Server...");

var minecraftService = new MinecraftServerService(ServerProfile);

minecraftService.OutputReceived += message => { Console.WriteLine(message); };

bool started = await minecraftService.StartAsync();

if (!started)
{
    Console.WriteLine("Failed to start server...!!");
}

var notification = new NotificationService();
string stuff = await notification.SendNotification($"Server {ServerProfile.Name} has started!!");
Console.WriteLine(stuff);

await minecraftService.WaitForExitAsync();
