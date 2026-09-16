using System.Diagnostics;
using Mg.Tasks;

namespace Mg;

public sealed class TaskRunner
{
    private readonly IReadOnlyDictionary<string, IMgTask> tasks;
    private readonly string projectPath;

    public TaskRunner(string projectPath)
    {
        this.projectPath = projectPath;
        tasks = new Dictionary<string, IMgTask>(StringComparer.OrdinalIgnoreCase)
        {
            ["build"] = new BuildTask(),
            ["run"] = new RunTask(),
            ["publish"] = new PublishTask()
        };
    }

    public int Run(string taskName)
    {
        if (!tasks.TryGetValue(taskName, out IMgTask? task))
        {
            throw new InvalidOperationException($"Unknown task: {taskName}");
        }

        IReadOnlyList<string> arguments = task.GetArguments(projectPath);

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