// See https://aka.ms/new-console-template for more information
using MyFirstTelegramBot;
using MyFirstTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


Console.WriteLine("Hello, World!");
Config config = new Config();

if (String.IsNullOrEmpty(config.Token))
{
    Console.WriteLine("Токен не найден!");
    return;
}

var cts = new CancellationTokenSource();
var botClient = new TelegramBotClient(config.Token);
var receiverOptions = new ReceiverOptions
{
AllowedUpdates = [UpdateType.Message],
DropPendingUpdates = true
};
var handler = new UpdateHandler(); 
handler.OnHandleUpdateStarted += Handler_OnHandleUpdateStarted;
handler.OnHandleUpdateEnd += Handler_OnHandleUpdateEnd;
handler.RegisterHandler(LogMessage);
botClient.StartReceiving(handler, receiverOptions);

try
{
    Console.WriteLine("Нажмите английскую клавишу 'A' для выхода");
    var me = await botClient.GetMe();
    Console.WriteLine($"{me.Username} запущен!");

    while (!Console.KeyAvailable)
    {
        if (Console.ReadKey().Key == ConsoleKey.A)
        {
            Console.WriteLine("Выход...");
            cts.Cancel();
            break;
        }
        else
        {
            var info = await botClient.GetMeAsync();
            Console.WriteLine($"Информация о боте: {info.Username}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Произошла критическая ошибка! \nОписание ошибки: " + ex.ToString());
}
finally
{
    handler.OnHandleUpdateStarted -= Handler_OnHandleUpdateStarted;
    handler.OnHandleUpdateEnd -= Handler_OnHandleUpdateEnd;
}

void Handler_OnHandleUpdateEnd(object? sender, string e)
{
    Console.WriteLine($"Закончилась обработка сообщения '{e}'");
}

static void Handler_OnHandleUpdateStarted(object? sender, string e)
{
    Console.WriteLine($"Началась обработка сообщения '{e}'");
}
static void LogMessage(string message) => Console.WriteLine(message);

public delegate void MessageHandler(string message);

