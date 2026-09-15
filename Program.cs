using Mg;

if (args.Length == 0)
{
	Console.Error.WriteLine("Usage: mg <build|run|publish> [...]");
	return 1;
}

try
{
	MgConfig config = MgConfig.Load(Path.Combine(Directory.GetCurrentDirectory(), "mg.config"));
	var taskRunner = new TaskRunner(config.ProjectPath);

	foreach (string taskName in args)
	{
		Console.WriteLine($"== {taskName} ==");
		int exitCode = taskRunner.Run(taskName.ToLowerInvariant());
		if (exitCode != 0)
		{
			Console.Error.WriteLine($"Task '{taskName}' failed with exit code {exitCode}.");
			return exitCode;
		}
	}

	return 0;
}
catch (Exception exception) when (exception is InvalidOperationException or IOException)
{
	Console.Error.WriteLine($"Error: {exception.Message}");
	return 1;
}
