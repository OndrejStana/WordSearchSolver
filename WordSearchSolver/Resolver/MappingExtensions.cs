namespace WordSearchSolver.Resolver;

internal static class MappingExtensions
{
    internal static Grid ToGrid(this List<string> matrix)
    {
        var rows = matrix.Count;
        var cols = matrix.FirstOrDefault()?.Length ?? 0;

        var grid = new Grid(rows, cols);

        for (var rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            var rowText = matrix[rowIndex];
            for (var colIndex = 0; colIndex < cols; colIndex++)
            {
                grid[rowIndex, colIndex] = new Cell
                {
                    Row = rowIndex,
                    Col = colIndex,
                    Character = rowText[colIndex],
                    IsCrossed = false
                };
            }
        }

        return grid;
    }
}