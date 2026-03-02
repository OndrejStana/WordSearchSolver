namespace WordSearchSolver.Serializer;

public class JsonInput
{
    public List<string> Matrix { get; set; } = null!;

    public List<string> Words { get; set; } = null!;

    public bool CrossOnlyFirstOccurence { get; set; } = false;
}