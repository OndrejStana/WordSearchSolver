using Microsoft.Extensions.Logging;
using Moq;
using WordSearchSolver.Resolver;
using WordSearchSolver.Serializer;

namespace WordSearchSolver.Tests;

[TestFixture]
public class WordSearchApplicationTests
{
    private Mock<IJsonReaderService> _jsonReaderServiceMock = null!;
    private Mock<IJsonValidatorService> _jsonValidatorServiceMock = null!;
    private Mock<IResolverService> _resolverServiceMock = null!;
    private Mock<ILogger<WordSearchApplication>> _loggerMock = null!;
    private WordSearchApplication _wordSearchApplication = null!;

    [SetUp]
    public void Setup()
    {
        _jsonReaderServiceMock = new Mock<IJsonReaderService>();
        _jsonValidatorServiceMock = new Mock<IJsonValidatorService>();
        _resolverServiceMock = new Mock<IResolverService>();
        _loggerMock = new Mock<ILogger<WordSearchApplication>>();

        _wordSearchApplication = new WordSearchApplication(
            _jsonReaderServiceMock.Object,
            _jsonValidatorServiceMock.Object,
            _loggerMock.Object,
            _resolverServiceMock.Object);
    }

    [Test]
    public async Task RunAsync_NoArgs_LogsUsageMessage()
    {
        // Act
        await _wordSearchApplication.RunAsync(Array.Empty<string>());

        // Assert
        VerifyLogMessage(LogLevel.Information, WordSearchApplication.UsageMessage, Times.Once());
    }

    [Test]
    public async Task RunAsync_InvalidJson_LogsValidationFailedMessage()
    {
        // Arrange
        var filePath = "test.json";
        var input = new JsonInput();
        _jsonReaderServiceMock.Setup(s => s.LoadAsync(filePath)).ReturnsAsync(input);
        _jsonValidatorServiceMock.Setup(s => s.Validate(input))
            .Returns((false, ["Error"]));

        // Act
        await _wordSearchApplication.RunAsync([filePath]);

        // Assert
        VerifyLogMessage(LogLevel.Error, WordSearchApplication.ValidationFailedMessage.Replace("{Errors}", "Error"), Times.Once());
    }

    [Test]
    public async Task RunAsync_ExceptionThrown_LogsErrorOccurredMessage()
    {
        // Arrange
        var filePath = "test.json";
        _jsonReaderServiceMock.Setup(s => s.LoadAsync(filePath)).ThrowsAsync(new Exception("Test error"));

        // Act
        await _wordSearchApplication.RunAsync([filePath]);

        // Assert
        VerifyLogMessage(LogLevel.Error, WordSearchApplication.ErrorOccurredMessage.Replace("{Message}", "Test error"), Times.Once());
    }

    [Test]
    public async Task RunAsync_ValidInput_LogsResultMessageAndSetsResult()
    {
        // Arrange
        var filePath = "test.json";
        var input = new JsonInput { Matrix = new List<string> { "ABC" }, Words = new List<string> { "A" } };
        var resolvedResult = "ABC";
        
        _jsonReaderServiceMock.Setup(s => s.LoadAsync(filePath)).ReturnsAsync(input);
        _jsonValidatorServiceMock.Setup(s => s.Validate(input)).Returns((true, Array.Empty<string>()));
        
        _resolverServiceMock
            .Setup(s => s.Resolve(It.IsAny<Grid>(), input.Words, input.CrossOnlyFirstOccurence))
            .Returns(resolvedResult);

        // Act
        await _wordSearchApplication.RunAsync([filePath]);

        // Assert
        VerifyLogMessage(LogLevel.Information, WordSearchApplication.ResultMessage.Replace("{Result}", resolvedResult), Times.Once());
    }

    private void VerifyLogMessage(LogLevel logLevel, string expectedMessage, Times times)
    {
        _loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == expectedMessage),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }
}
