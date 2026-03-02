using Microsoft.Extensions.Logging;
using Moq;
using WordSearchSolver.Resolver;
using WordSearchSolver.Serializer;

namespace WordSearchSolver.Tests.Resolver;

[TestFixture]
public class ResolverServiceTests
{
    private Mock<ILogger<ResolverService>> _loggerMock = null!;
    private ResolverService _resolverService = null!;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<ResolverService>>();
        _resolverService = new ResolverService(_loggerMock.Object);
    }

    [Test]
    public void Resolve_GivenValidMatrixAndWords_ShouldReturnUncrossedCharacters()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = new List<string>
            {
                "ABCDE",
                "FGHIJ",
                "KLMNO",
                "PQRST",
                "UVWXY"
            },
            Words = new List<string>
            {
                "AGMSY", // Diagonal
                "CHM",   // Diagonal
                "PKF",   // Vertical upwards
                "XWV"    // Horizontal backwards
            },
            CrossOnlyFirstOccurence = false
        };

        // Act
        var result = _resolverService.Resolve(input.Matrix.ToGrid(), input.Words, input.CrossOnlyFirstOccurence);

        // Assert
        Assert.That(result, Is.EqualTo("BDEIJLNOQRTU"));
    }

    [Test]
    public void Resolve_GivenAssigmentMatrixAndWords_ShouldReturnUncrossedCharacters()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = new List<string>
            {
                "KALTJSHODA",
                "LLPUKLTOAT",
                "AKTAAKAARR",
                "SAANLAKPEA",
                "ARPOVPTOKK",
                "RHOMOLICEA",
                "KOLSPEKESR",
                "ORAOCAALTP",
                "SPOKVSTIAA",
                "MATKAFTKAT",
                "AIAKOSTKAY"
            },
            Words = new List<string>
            {
                "ALKA", "HORA", "JUTA", "KAPLE", "KARPATY", "KARTA", "KASA", "KAVKA",
                "KLAS", "KOSMONAUT", "KOST", "KROK", "LAPKA", "MATKA", "OKRASA", "OPAT",
                "OSMA", "PAKT", "PATKA", "PIETA", "POCEL", "POVLAK", "PROHRA", "SEKERA",
                "SHODA", "SOPKA", "TAKT", "TAKTIKA", "TLAK", "VOLHA"
            },
            CrossOnlyFirstOccurence = false
        };

        // Act
        var result = _resolverService.Resolve(input.Matrix.ToGrid(), input.Words, input.CrossOnlyFirstOccurence);

        // Assert
        Assert.That(result, Is.EqualTo("PACIFIK"));
    }

    [Test]
    public void Resolve_GivenCrossOnlyFirstOccurenceFalse_ShouldCrossAllOccurrencesOfWord()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = new List<string>
            {
                "CAT",
                "XXX",
                "CAT"
            },
            Words = new List<string> { "CAT" },
            CrossOnlyFirstOccurence = false
        };

        // Act
        var result = _resolverService.Resolve(input.Matrix.ToGrid(), input.Words, input.CrossOnlyFirstOccurence);

        // Assert
        Assert.That(result, Is.EqualTo("XXX"));
    }

    [Test]
    public void Resolve_GivenCrossOnlyFirstOccurenceTrue_ShouldCrossOnlyFirstOccurrenceOfWord()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = new List<string>
            {
                "CAT",
                "XXX",
                "CAT"
            },
            Words = new List<string> { "CAT" },
            CrossOnlyFirstOccurence = true
        };

        // Act
        var result = _resolverService.Resolve(input.Matrix.ToGrid(), input.Words, input.CrossOnlyFirstOccurence);

        // Assert
        Assert.That(result, Is.EqualTo("XXXCAT"));
    }

    [Test]
    public void Resolve_GivenWordLongerThanMatrix_ShouldLogWarningAndReturnRemainingCharacters()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = new List<string>
            {
                "ABC",
                "DEF",
                "GHI"
            },
            Words = new List<string> { "ABCD" }
        };

        // Act
        var result = _resolverService.Resolve(input.Matrix.ToGrid(), input.Words, input.CrossOnlyFirstOccurence);

        // Assert
        Assert.That(result, Is.EqualTo("ABCDEFGHI"));
        VerifyLogMessage(LogLevel.Warning, string.Format(ResolverService.WordNotFoundMessageFormat, "ABCD"), Times.Once());
    }

    [Test]
    public void Resolve_GivenWordNotFound_ShouldLogWarningAndReturnRemainingCharacters()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = new List<string>
            {
                "ABC",
                "DEF",
                "GHI"
            },
            Words = new List<string> { "XYZ" }
        };

        // Act
        var result = _resolverService.Resolve(input.Matrix.ToGrid(), input.Words, input.CrossOnlyFirstOccurence);

        // Assert
        Assert.That(result, Is.EqualTo("ABCDEFGHI"));
        VerifyLogMessage(LogLevel.Warning, string.Format(ResolverService.WordNotFoundMessageFormat, "XYZ"), Times.Once());
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