namespace Mg.Tasks;

public sealed class PublishTask : IMgTask
{
    public string Name => "publish";

    public IReadOnlyList<string> GetArguments(string projectPath) =>
        ["publish", "-c", "Release", projectPath];
}