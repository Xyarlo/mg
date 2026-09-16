namespace Mg.Tasks;

public interface IMgTask
{
    string Name { get; }

    IReadOnlyList<string> GetArguments(string projectPath);
}