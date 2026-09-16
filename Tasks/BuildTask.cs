namespace Mg.Tasks;

public sealed class BuildTask : IMgTask
{
    public string Name => "build";

    public IReadOnlyList<string> GetArguments(string projectPath) =>
        ["build", "-c", "Debug", projectPath];
}