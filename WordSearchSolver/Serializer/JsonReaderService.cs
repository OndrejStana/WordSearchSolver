using System.Text.Json;

namespace WordSearchSolver.Serializer;

public class JsonReaderService : IJsonReaderService
{
    internal const string FileNotFoundMessage = "The file at {0} was not found.";

    internal const string InvalidDataMessage = "The JSON file is empty or could not be deserialized into the expected structure.";

    public async Task<JsonInput> LoadAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(string.Format(FileNotFoundMessage, filePath));
        }

        await using var stream = File.OpenRead(filePath);
        var result = await JsonSerializer.DeserializeAsync<JsonInput>(stream);
        return result ?? throw new InvalidDataException(InvalidDataMessage);
    }
}