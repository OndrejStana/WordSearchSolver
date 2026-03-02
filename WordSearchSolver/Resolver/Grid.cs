using System.Collections;

namespace WordSearchSolver.Resolver;

/// <summary>
/// Represents the word search puzzle grid, consisting of a 2D array of character cells.
/// </summary>
public class Grid : IEnumerable<Cell>
{
    private readonly Cell[,] _cells;

    public int Rows => _cells.GetLength(0);
    public int Cols => _cells.GetLength(1);

    public Grid(int rows, int cols)
    {
        _cells = new Cell[rows, cols];
    }

    public Cell this[int row, int col]
    {
        get => _cells[row, col];
        set => _cells[row, col] = value;
    }

    public IEnumerator<Cell> GetEnumerator()
    {
        foreach (var cell in _cells)
        {
            yield return cell;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}