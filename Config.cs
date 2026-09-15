namespace Mg;

public sealed class MgConfig
{
    public required string ProjectPath { get; init; }

    public static MgConfig Load(string configPath)
    {
        if (!File.Exists(configPath))
        {
            throw new InvalidOperationException($"Configuration file not found: {configPath}");
        }

        string? section = null;
        string? projectPath = null;

        foreach (string rawLine in File.ReadLines(configPath))
        {
            string line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith(';') || line.StartsWith('#'))
            {
                continue;
            }

            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                section = line[1..^1].Trim();
                continue;
            }

            int separator = line.IndexOf('=');
            if (separator < 0 || !string.Equals(section, "project", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string key = line[..separator].Trim();
            string value = line[(separator + 1)..].Trim();
            if (string.Equals(key, "path", StringComparison.OrdinalIgnoreCase))
            {
                projectPath = value;
            }
        }

        if (string.IsNullOrWhiteSpace(projectPath))
        {
            throw new InvalidOperationException($"The [project] path setting is missing in {configPath}");
        }

        string configDirectory = Path.GetDirectoryName(Path.GetFullPath(configPath))!;
        string resolvedProjectPath = Path.GetFullPath(projectPath, configDirectory);
        if (!File.Exists(resolvedProjectPath))
        {
            throw new InvalidOperationException($"Project file not found: {resolvedProjectPath}");
        }

        return new MgConfig { ProjectPath = resolvedProjectPath };
    }
}