namespace WordSearchSolver;

/// <summary>
/// Defines the main entry point and runner for the word search application.
/// </summary>
public interface IWordSearchApplication
{
    /// <summary>
    /// Executes the word search application logic given the command-line arguments.
    /// </summary>
    /// <param name="args">The command-line arguments containing the path to the input JSON file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RunAsync(string[] args);
}