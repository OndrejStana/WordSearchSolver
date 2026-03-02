namespace WordSearchSolver.Serializer;

public class JsonValidatorService : IJsonValidatorService
{
    internal const string MinRowsError = "Matrix must have at least 2 rows.";
    internal const string MinRowLengthError = "Rows in Matrix must have at least 2 characters.";
    internal const string NonRectangularMatrixError = "Matrix is not rectangular; row lengths vary.";
    internal const string NonLetterRowError = "Row {0} contains non-letter characters.";
    internal const string NullOrEmptyWordError = "Word at position {0} is empty or null.";
    internal const string NonLetterWordError = "Word '{0}' contains non-letter characters.";
    internal const string MissingMatrixError = "Matrix is missing from JSON.";
    internal const string MissingWordsError = "Words are missing from JSON.";

    public (bool IsValid, string[] Errors) Validate(JsonInput input)
    {
        var errors = GetValidationErrors(input).ToArray();
        return (errors.Length == 0, errors);
    }

    private static IEnumerable<string> GetValidationErrors(JsonInput input)
    {
        if (input.Matrix == null)
        {
            yield return MissingMatrixError;
        }
        
        if (input.Words == null)
        {
            yield return MissingWordsError;
        }

        if (input.Matrix == null || input.Words == null)
        {
            yield break;
        }

        foreach (var error in CheckStructure(input))
        {
            yield return error;
        }

        foreach (var error in CheckCharacters(input))
        {
            yield return error;
        }
    }

    private static IEnumerable<string> CheckStructure(JsonInput input)
    {
        if (input.Matrix.Count < 2)
        {
            yield return MinRowsError;
        }

        if (input.Matrix.Any(row => row.Length < 2))
        {
            yield return MinRowLengthError;
        }

        if (input.Matrix.Select(r => r.Length).Distinct().Count() > 1)
        {
            yield return NonRectangularMatrixError;
        }
    }

    private static IEnumerable<string> CheckCharacters(JsonInput input)
    {
        for (var i = 0; i < input.Matrix.Count; i++)
        {
            if (input.Matrix[i].Any(c => !char.IsLetter(c)))
            {
                yield return string.Format(NonLetterRowError, i);
            }
        }

        for (var i = 0; i < input.Words.Count; i++)
        {
            var word = input.Words[i];

            if (string.IsNullOrWhiteSpace(word))
            {
                yield return string.Format(NullOrEmptyWordError, i);
            }
            else if (word.Any(c => !char.IsLetter(c)))
            {
                yield return string.Format(NonLetterWordError, word);
            }
        }
    }
}