namespace WordSearchSolver.Serializer;

/// <summary>
/// Defines a service responsible for validating the JSON input provided to the word search solver.
/// </summary>
public interface IJsonValidatorService
{
    /// <summary>
    /// Validates the provided JSON input to ensure it meets the requirements for the word search solver.
    /// </summary>
    /// <param name="input">The JSON input containing the matrix and words.</param>
    /// <returns>A tuple indicating whether the input is valid and an array of error messages if it is not.</returns>
    (bool IsValid, string[] Errors) Validate(JsonInput input);
}