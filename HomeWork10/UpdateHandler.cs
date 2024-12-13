using System.Net.Http.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HomeWork10
{
    internal class UpdateHandler : IUpdateHandler
    {
        public delegate void MessageHandler(Message message);
        public event MessageHandler OnHandleUpdateStarted;
        public event MessageHandler OnHandleUpdateCompleted;
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception);
            return Task.CompletedTask;
        }
        record CatFactDto(string Fact, int length);
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            OnHandleUpdateStarted?.Invoke(update.Message);
            if (update.Type == UpdateType.Message)
            {
                if(update.Message.Text == "/cat")
                {
                    using var client = new HttpClient();
                    var catFact = await client.GetFromJsonAsync<CatFactDto>("https://catfact.ninja/fact", cancellationToken);
                    botClient.SendMessage(update.Message.Chat.Id, catFact.Fact);
                }
                await botClient.SendMessage(update.Message.Chat.Id, "Сообщение успешно принято");
            }
            OnHandleUpdateCompleted?.Invoke(update.Message);
        }
    }
}
