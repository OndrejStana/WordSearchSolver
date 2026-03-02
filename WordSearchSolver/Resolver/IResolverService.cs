namespace WordSearchSolver.Resolver;

/// <summary>
/// Defines the core logic for resolving a word search puzzle given a grid and a set of words.
/// </summary>
public interface IResolverService
{
    /// <summary>
    /// Resolves the word search by finding the specified words within the given grid and returning the uncrossed characters.
    /// </summary>
    /// <param name="grid">The grid represented as a collection of cells.</param>
    /// <param name="words">The list of words to search for in the grid.</param>
    /// <param name="crossOnlyFirstOccurence">If true, only crosses out the first occurrence of each word; otherwise, crosses out all occurrences.</param>
    /// <returns>A string containing the concatenated characters that were not crossed out.</returns>
    string Resolve(Grid grid, List<string> words, bool crossOnlyFirstOccurence);
}