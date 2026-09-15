using System.Diagnostics;

namespace Mg;

public sealed class TaskRunner
{
    private readonly string projectPath;

    public TaskRunner(string projectPath)
    {
        this.projectPath = projectPath;
    }

    public int Run(string taskName)
    {
        var arguments = taskName switch
        {
            "build" => new[] { "build", "-c", "Debug", projectPath },
            "run" => new[] { "run", "-c", "Debug", "--no-build", "--project", projectPath },
            "publish" => new[] { "publish", "-c", "Release", projectPath },
            _ => throw new InvalidOperationException($"Unknown task: {taskName}")
        };

        Console.WriteLine($"> dotnet {string.Join(' ', arguments.Select(QuoteArgument))}");

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        foreach (string argument in arguments)
        {
            process.StartInfo.ArgumentList.Add(argument);
        }

        process.OutputDataReceived += (_, eventArgs) =>
        {
            if (eventArgs.Data is not null)
            {
                Console.Out.WriteLine(eventArgs.Data);
            }
        };
        process.ErrorDataReceived += (_, eventArgs) =>
        {
            if (eventArgs.Data is not null)
            {
                Console.Error.WriteLine(eventArgs.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        return process.ExitCode;
    }

    private static string QuoteArgument(string argument) =>
        argument.Contains(' ') ? $"\"{argument}\"" : argument;
}