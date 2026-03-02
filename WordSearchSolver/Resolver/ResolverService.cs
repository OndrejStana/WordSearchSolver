using Microsoft.Extensions.Logging;

namespace WordSearchSolver.Resolver;

public class ResolverService : IResolverService
{
    internal const string WordNotFoundMessageFormat = "Word '{0}' was not found in the matrix.";
    
    private readonly ILogger<ResolverService> _logger;

    public ResolverService(ILogger<ResolverService> logger)
    {
        _logger = logger;
    }

    public string Resolve(Grid grid, List<string> words, bool crossOnlyFirstOccurence)
    {
        foreach (var word in words)
        {
            var found = false;

            foreach (var cell in grid.Where(cell => cell.Character == word[0]))
            {
                if (found && crossOnlyFirstOccurence)
                {
                    break;
                }

                foreach (var searchDirection in SearchDirection.All)
                {
                    if (CheckWord(grid, word, cell, searchDirection))
                    {
                        found = true;
                        if (crossOnlyFirstOccurence)
                        {
                            break;
                        }
                    }
                }
            }

            if (!found)
            {
                _logger.LogWarning(WordNotFoundMessageFormat, word);
            }
        }

        return string.Concat(grid.Where(c => !c.IsCrossed).Select(c => c.Character));
    }

    private static bool CheckWord(Grid grid, string word, Cell startingCell, SearchDirection searchDirection)
    {
        for (var i = 0; i < word.Length; i++)
        {
            var row = startingCell.Row + i * searchDirection.RowOffset;
            var col = startingCell.Col + i * searchDirection.ColOffset;

            if (row < 0 || row >= grid.Rows || col < 0 || col >= grid.Cols)
            {
                return false;
            }

            var cell = grid[row, col];
            if (cell.Character != word[i])
            {
                return false;
            }
        }

        CrossWord(grid, word.Length, startingCell, searchDirection);
        return true;
    }

    private static void CrossWord(Grid grid, int length, Cell startingCell, SearchDirection searchDirection)
    {
        for (var i = 0; i < length; i++)
        {
            var row = startingCell.Row + i * searchDirection.RowOffset;
            var col = startingCell.Col + i * searchDirection.ColOffset;

            var cell = grid[row, col];
            cell.IsCrossed = true;
            grid[row, col] = cell;
        }
    }
}