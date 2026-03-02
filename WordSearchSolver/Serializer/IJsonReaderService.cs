namespace WordSearchSolver.Serializer;

/// <summary>
/// Defines a service responsible for reading and parsing JSON input files.
/// </summary>
public interface IJsonReaderService
{
    /// <summary>
    /// Asynchronously loads and parses the JSON input from the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the JSON file to explicitly load.</param>
    /// <returns>A task that represents the asynchronous loading operation. The task result contains the parsed JsonInput.</returns>
    Task<JsonInput> LoadAsync(string filePath);
}