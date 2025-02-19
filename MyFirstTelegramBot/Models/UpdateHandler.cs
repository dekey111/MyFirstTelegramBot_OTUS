using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace MyFirstTelegramBot.Models
{
    public class UpdateHandler : IUpdateHandler
    {
        MessageHandler? taken;

        public event EventHandler<string> OnHandleUpdateStarted;
        public event EventHandler<string> OnHandleUpdateEnd;

        public record CatFactDto(string Fact, int Length);

        public void RegisterHandler(MessageHandler del)
        {
            taken = del;
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            throw new Exception(exception.Message);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if(update.Message?.From is null)
                throw new Exception("Пустое сообщение!");

            string message = update.Message.Text.Trim();
            if (message.ToLower().Contains("/cat"))
            {
                var catFact = await GetRandomCatFact(cancellationToken);
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Случайный факт о кошках: {catFact.Fact}", cancellationToken: cancellationToken);
            }

            OnHandleUpdateStarted.Invoke(this, message);

            var info = $"{update.Message.From.Username}: {message}";
            taken?.Invoke(info);

            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Сообщение успешно принято", cancellationToken: cancellationToken);

            OnHandleUpdateEnd.Invoke(this, message);
        }

        private static async Task<CatFactDto> GetRandomCatFact(CancellationToken cancellationToken)
        {
            var cts = new CancellationTokenSource();
            using var client = new HttpClient();
            var catFact = await client.GetFromJsonAsync<CatFactDto>("https://catfact.ninja/fact", cts.Token);
            return catFact;
        }
    }
}
