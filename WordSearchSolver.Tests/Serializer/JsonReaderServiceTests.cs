using System.Text.Json;
using WordSearchSolver.Serializer;

namespace WordSearchSolver.Tests.Serializer;

[TestFixture]
public class JsonReaderServiceTests
{
    private JsonReaderService _jsonReaderService = null!;
    private string _testFilePath = null!;

    [SetUp]
    public void Setup()
    {
        _jsonReaderService = new JsonReaderService();
        _testFilePath = Path.Combine(Path.GetTempPath(), $"wordsearch_test_{Guid.NewGuid()}.json");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Test]
    public void LoadAsync_FileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<FileNotFoundException>(() => _jsonReaderService.LoadAsync("non_existent_file.json"));
        Assert.That(ex?.Message, Is.EqualTo(string.Format(JsonReaderService.FileNotFoundMessage, "non_existent_file.json")));
    }

    [Test]
    public void LoadAsync_EmptyFile_ThrowsInvalidDataException()
    {
        // Arrange
        File.WriteAllText(_testFilePath, "");

        // Act & Assert
        Assert.ThrowsAsync<JsonException>(() => _jsonReaderService.LoadAsync(_testFilePath));
    }

    [Test]
    public async Task LoadAsync_ValidJson_ReturnsParsedObject()
    {
        // Arrange
        var jsonContent = @"{ ""Matrix"": [ ""ABC"", ""DEF"" ], ""Words"": [ ""HELLO"", ""WORLD"" ] }";
        await File.WriteAllTextAsync(_testFilePath, jsonContent);

        // Act
        var result = await _jsonReaderService.LoadAsync(_testFilePath);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Matrix.Count, Is.EqualTo(2));
        Assert.That(result.Words.Count, Is.EqualTo(2));
    }
}
