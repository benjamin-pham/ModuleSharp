using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Contract.Utilities;

public static class LoggerExtensions
{
    public static void LogWithCaller(
        this ILogger logger,
        string messageTemplate,
        object?[]? args,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0,
        [CallerMemberName] string member = "")
    {
        LogInternal(logger, LogLevel.Information, null, messageTemplate, args, file, line, member);
    }
    public static void LogWithCaller(
        this ILogger logger,
        object?[]? args,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0,
        [CallerMemberName] string member = "")
    {
        LogInternal(logger, LogLevel.Information, null, "", args, file, line, member);
    }
    public static void LogWithCaller(
        this ILogger logger,
        Exception exception,
        string messageTemplate,
        object?[]? args = null,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0,
        [CallerMemberName] string member = "")
    {
        LogInternal(logger, LogLevel.Error, exception, messageTemplate, args, file, line, member);
    }

    public static void LogWithCaller(
        this ILogger logger,
        LogLevel level,
        string messageTemplate,
        object?[]? args,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0,
        [CallerMemberName] string member = "")
    {
        LogInternal(logger, level, null, messageTemplate, args, file, line, member);
    }

    public static void LogWithCaller(
        this ILogger logger,
        LogLevel level,
        Exception exception,
        string messageTemplate,
        object?[]? args,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0,
        [CallerMemberName] string member = "")
    {
        LogInternal(logger, level, exception, messageTemplate, args, file, line, member);
    }
    private static readonly Regex PlaceholderRegex = new(@"\{(?<key>[^\}]+)\}", RegexOptions.Compiled);
    private static void LogInternal(
        this ILogger logger,
        LogLevel level,
        Exception? exception,
        string messageTemplate,
        object?[]? args,
        string file = "",
        int line = 0,
        string member = ""
        )
    {
        var displayFile = file;
        var idx = file.IndexOf("\\src\\", StringComparison.OrdinalIgnoreCase);
        if (idx >= 0)
        {
            displayFile = file[idx..];
        }

        var template = $"at {{SourceFile}}:{{LineNumber}} {{Member}}\n{messageTemplate}";

        var placeholders = messageTemplate.Contains('{')
            ? [.. PlaceholderRegex.Matches(messageTemplate).Select(m => m.Groups["key"].Value)]
            : Array.Empty<string>();

        var placeholderCount = placeholders.Length;

        int extraCount = exception != null ? 2 : 0;
        const int callerArgCount = 3;
        args ??= [];
        var newArgs = new object?[callerArgCount + placeholderCount + extraCount];
        newArgs[0] = displayFile;
        newArgs[1] = line;
        newArgs[2] = member;

        for (int i = 0; i < placeholderCount; i++)
        {
            if (i < args.Length)
                newArgs[callerArgCount + i] = args[i];
            else
                newArgs[callerArgCount + i] = $"{{{placeholders[i]}:unknown}}";
        }

        if (exception != null)
        {
            var innerEx = exception.InnerException;
            var baseEx = exception.GetBaseException();
            newArgs[callerArgCount + placeholderCount] = innerEx;
            newArgs[callerArgCount + placeholderCount + 1] = baseEx;
            template += "\nInnerException:{InnerException}\nBaseException:{BaseException}";
            logger.Log(level, exception, template, newArgs);
        }
        else
        {
            logger.Log(level, template, newArgs);
        }
    }
}