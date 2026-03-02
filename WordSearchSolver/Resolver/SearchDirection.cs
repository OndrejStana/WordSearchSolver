namespace WordSearchSolver.Resolver;

/// <summary>
/// Defines a vector for searching words within the grid using row and column offsets.
/// </summary>
internal readonly record struct SearchDirection(int RowOffset, int ColOffset)
{
    private static readonly SearchDirection TopLeft = new(-1, -1);
    private static readonly SearchDirection Top = new(-1, 0);
    private static readonly SearchDirection TopRight = new(-1, 1);
    private static readonly SearchDirection Left = new(0, -1);
    private static readonly SearchDirection Right = new(0, 1);
    private static readonly SearchDirection BottomLeft = new(1, -1);
    private static readonly SearchDirection Bottom = new(1, 0);
    private static readonly SearchDirection BottomRight = new(1, 1);

    internal static readonly SearchDirection[] All =
    [
        TopLeft, Top, TopRight,
        Left, Right,
        BottomLeft, Bottom, BottomRight
    ];
}