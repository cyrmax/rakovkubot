using System.Security;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RakovkuBot;

class Program
{
    public static async Task Main(string[] args)
    {
        string? botToken = Environment.GetEnvironmentVariable("RAKOVKU_BOT_TOKEN");
        if (botToken is null) return;
        var bot = new TelegramBotClient(botToken);
        using CancellationTokenSource cts = new CancellationTokenSource();
        ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
        bot.StartReceiving(updateHandler: botOnUpdate, pollingErrorHandler: botOnError, receiverOptions: receiverOptions, cancellationToken: cts.Token);
        var me = await bot.GetMeAsync();
        Console.WriteLine($"Started Rakovku bot. User ID is {me.Id}, username is {me.Username}");
        bool shouldTerminate = false;
        Console.CancelKeyPress += new ConsoleCancelEventHandler((sender, args) =>
        {
            Console.WriteLine("Ctrl+C received. Terminating.");
            cts.Cancel();
            shouldTerminate = true;
        });
        while (!shouldTerminate) { }
    }

    private static async Task botOnUpdate(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message) return;
        if (update.Message?.Text is not { } messageText) return;
        string result = await Converter.ConvertAsync(messageText, cancellationToken);
        await bot.SendTextMessageAsync(update.Message.Chat, result, replyToMessageId: update.Message.MessageId);
    }

    private static async Task botOnError(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"An error has occured.\n{exception.Message}");
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
            if (lowerConsonants.Contains(c))
            {
                result.Append(lowerConsonants[lowerConsonants.Length - 1 - lowerConsonants.IndexOf(c)]);
                continue;
            }
            if (upperConsonants.Contains(c))
            {
                result.Append(upperConsonants[upperConsonants.Length - 1 - upperConsonants.IndexOf(c)]);
                continue;
            }
            result.Append(c);
        }
        return result.ToString();
    }
}