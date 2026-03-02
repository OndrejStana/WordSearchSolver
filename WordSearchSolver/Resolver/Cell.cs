namespace WordSearchSolver.Resolver;

/// <summary>
/// Represents a single character cell within a word search puzzle grid.
/// </summary>
public struct Cell
{
    public int Col { get; init; }
    public int Row { get; init; }
    public char Character { get; init; }
    public bool IsCrossed { get; set; }
}