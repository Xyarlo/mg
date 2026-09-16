namespace Mg.Tasks;

public sealed class RunTask : IMgTask
{
    public string Name => "run";

    public IReadOnlyList<string> GetArguments(string projectPath) =>
        ["run", "-c", "Debug", "--no-build", "--project", projectPath];
}