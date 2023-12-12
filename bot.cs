using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;

namespace RakovkuBot;

class Program
{
    private static ILoggerFactory? loggerFactory;
    private static ILogger? logger;
    public static async Task Main(string[] args)
    {
        loggerFactory = LoggerFactory.Create((ILoggingBuilder builder) =>
        {
            builder
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole();
        });
        logger = loggerFactory?.CreateLogger("RakovkuBot");
        logger?.LogInformation("Initializing Rakovku bot");
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((object sender, UnhandledExceptionEventArgs args) =>
        {
            logger?.LogCritical((args.ExceptionObject as Exception), "An unhandled exception occured");
        });
        string? botToken = Environment.GetEnvironmentVariable("RAKOVKU_BOT_TOKEN");
        if (botToken is null)
        {
            logger?.LogCritical("Unable to get RAKOVKU_BOT_TOKEN environment variable");
            return;
        }
        var bot = new TelegramBotClient(botToken);
        using CancellationTokenSource cts = new CancellationTokenSource();
        ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
        bot.StartReceiving(updateHandler: botOnUpdate, pollingErrorHandler: botOnError, receiverOptions: receiverOptions, cancellationToken: cts.Token);
        var me = await bot.GetMeAsync();
        logger?.LogInformation("Rakovku Bot started. Bot username: {}, bot user ID: {}", me.Username, me.Id);
        bool shouldTerminate = false;
        Console.CancelKeyPress += new ConsoleCancelEventHandler((sender, args) =>
        {
            logger?.LogInformation("Ctrl+C received. Terminating.");
            cts.Cancel();
            loggerFactory?.Dispose();
            shouldTerminate = true;
        });
        while (!shouldTerminate) { }
    }

    private static async Task botOnUpdate(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        logger?.LogDebug("Received update of type {UpdateType}", update.Type);
        if (update.Type != UpdateType.Message) return;
        if (update.Message?.Text is not { } messageText) return;
        logger?.LogDebug("Converting text message");
        string result = await Converter.ConvertAsync(messageText, cancellationToken);
        logger?.LogDebug("Sending converted text message");
        await bot.SendTextMessageAsync(update.Message.Chat, result, replyToMessageId: update.Message.MessageId);
    }

    // Here we do not use `async` keyword because the function does not do any awaitable operations. But we still need to return Task because `Telegram.Bot` architecture needs it. So we return `Task.CompletedTask` as a sort of stub.
    private static Task botOnError(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        logger?.LogError(exception, "An error occured.");
        return Task.CompletedTask;
    }
}

class Converter
{
    private static string lowerConsonants = new string("бвгджзклмнпрстфхцчшщ");
    private static string upperConsonants = new string("БВГДЖЗКЛМНПРСТФХЦЧШЩ");

    public static async Task<string> ConvertAsync(string input, CancellationToken cancellationToken)
    {
        return await Task.Run(() => Convert(input), cancellationToken);
    }

    public static string Convert(string input)
    {
        StringBuilder result = new StringBuilder(input.Length);
        foreach (char c in input)
        {
            result.Append(convert(c));
        }
        return result.ToString();
    }

    private static char convert(char c)
    {
        if (lowerConsonants.Contains(c))
        {
            return lowerConsonants[lowerConsonants.Length - 1 - lowerConsonants.IndexOf(c)];
        }
        if (upperConsonants.Contains(c))
        {
            return upperConsonants[upperConsonants.Length - 1 - upperConsonants.IndexOf(c)];
        }
        return c;
    }

    private static char[] convert(char[] chars, int count)
    {
        char[] result = new char[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = convert(chars[i]);
        }
        return result;
    }
}