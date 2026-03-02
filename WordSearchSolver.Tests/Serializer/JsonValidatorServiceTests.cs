using WordSearchSolver.Serializer;

namespace WordSearchSolver.Tests.Serializer;

[TestFixture]
public class JsonValidatorServiceTests
{
    private JsonValidatorService _jsonValidatorServiceService = null!;

    [SetUp]
    public void Setup()
    {
        _jsonValidatorServiceService = new JsonValidatorService();
    }

    [Test]
    public void Validate_ValidInput_ReturnsTrue()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = ["ABC", "DEF"],
            Words = ["HELLO"]
        };

        // Act
        var (isValid, errors) = _jsonValidatorServiceService.Validate(input);

        // Assert
        Assert.That(isValid, Is.True);
        Assert.That(errors, Is.Empty);
    }

    [Test]
    public void Validate_MatrixLessThanTwoRows_ReturnsError()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = ["ABC"],
            Words = ["HELLO"]
        };

        // Act
        var (isValid, errors) = _jsonValidatorServiceService.Validate(input);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(errors, Contains.Item(JsonValidatorService.MinRowsError));
    }

    [Test]
    public void Validate_RowLengthLessThanTwo_ReturnsError()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = ["A", "B"],
            Words = ["HELLO"]
        };

        // Act
        var (isValid, errors) = _jsonValidatorServiceService.Validate(input);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(errors, Contains.Item(JsonValidatorService.MinRowLengthError));
    }

    [Test]
    public void Validate_RowsNotUniformLength_ReturnsError()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = ["ABC", "DEFG"],
            Words = ["HELLO"]
        };

        // Act
        var (isValid, errors) = _jsonValidatorServiceService.Validate(input);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(errors, Contains.Item(JsonValidatorService.NonRectangularMatrixError));
    }

    [Test]
    public void Validate_MatrixContainsNonLetters_ReturnsError()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = ["AB1", "DEF"],
            Words = ["HELLO"]
        };

        // Act
        var (isValid, errors) = _jsonValidatorServiceService.Validate(input);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(errors, Contains.Item(string.Format(JsonValidatorService.NonLetterRowError, 0)));
    }

    [Test]
    public void Validate_WordIsEmpty_ReturnsError()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = ["ABC", "DEF"],
            Words = [""]
        };

        // Act
        var (isValid, errors) = _jsonValidatorServiceService.Validate(input);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(errors, Contains.Item(string.Format(JsonValidatorService.NullOrEmptyWordError, 0)));
    }

    [Test]
    public void Validate_WordContainsNonLetters_ReturnsError()
    {
        // Arrange
        var input = new JsonInput
        {
            Matrix = ["ABC", "DEF"],
            Words = ["HELL0"]
        };

        // Act
        var (isValid, errors) = _jsonValidatorServiceService.Validate(input);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(errors, Contains.Item(string.Format(JsonValidatorService.NonLetterWordError, "HELL0")));
    }
}
