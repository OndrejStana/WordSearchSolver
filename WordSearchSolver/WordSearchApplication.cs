using Microsoft.Extensions.Logging;
using WordSearchSolver.Resolver;
using WordSearchSolver.Serializer;

namespace WordSearchSolver;

public class WordSearchApplication : IWordSearchApplication
{
    internal const string UsageMessage = "Usage: WordSearchSolver <path-to-json-file>";
    internal const string LoadingFileMessage = "Loading file: {FilePath}";
    internal const string ValidationFailedMessage = "Validation failed. Errors found:\n{Errors}";
    internal const string ErrorOccurredMessage = "An error occurred: {Message}";
    internal const string ResultMessage = "Result: {Result}";

    private readonly IJsonReaderService _jsonReaderService;
    private readonly IJsonValidatorService _jsonValidatorService;
    private readonly IResolverService _resolverService;
    private readonly ILogger<WordSearchApplication> _logger;

    public WordSearchApplication(
        IJsonReaderService jsonReaderService, 
        IJsonValidatorService jsonValidatorService,
        ILogger<WordSearchApplication> logger,
        IResolverService resolverService)
    {
        _jsonReaderService = jsonReaderService;
        _jsonValidatorService = jsonValidatorService;
        _logger = logger;
        _resolverService = resolverService;
    }

    public async Task RunAsync(string[] args)
    {
        if (args.Length != 1)
        {
            _logger.LogInformation(UsageMessage);
            return;
        }

        var filePath = args[0];

        try
        {
            _logger.LogInformation(LoadingFileMessage, filePath);
            var input = await _jsonReaderService.LoadAsync(filePath);
            var (isValid, errors) = _jsonValidatorService.Validate(input);
            if (!isValid)
            {
                _logger.LogError(ValidationFailedMessage, string.Join(",", errors));
                return;
            }
            
            var result = _resolverService.Resolve(input.Matrix.ToGrid(), input.Words, input.CrossOnlyFirstOccurence);
            _logger.LogInformation(ResultMessage, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorOccurredMessage, ex.Message);
        }
    }
}
