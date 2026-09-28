using MCServerManager.MCServerManager.Core.Services;
using MCServerManager.MCServerManager.Core;

Console.Write("Repository path: ");

string? repositoryPath = Console.ReadLine();

if (string.IsNullOrWhiteSpace(repositoryPath))
{
    Console.WriteLine("Invalid repository path.");
    return;
}

var git = new GitService(repositoryPath);

var result = await git.GetStatusAsync();

git.CheckGit();

if (result.Success)
{
    Console.WriteLine("Git status:");

    if (string.IsNullOrWhiteSpace(result.Output))
    {
        Console.WriteLine("Repository is clean.");
    }
    else
    {
        Console.WriteLine(result.Output);
    }
}
else
{
    Console.WriteLine("Git command failed:");
    Console.WriteLine(result.Error);
}