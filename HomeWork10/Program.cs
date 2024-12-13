using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HomeWork10
{
    internal class Program
    {
        public static string token { get; } = "<YOUR TOKEN HERE>";
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var botClient = new TelegramBotClient(token);
            var commands = new[] {
                new BotCommand { Command = "/cat",Description = "Случайный факт о кошках"},
            };
            await botClient.SetMyCommands(commands, cancellationToken: cts.Token);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message],
                DropPendingUpdates = true
            };
            var handler = new UpdateHandler();
            handler.OnHandleUpdateStarted += (Message message) => Console.WriteLine($"Началась обработка сообщения {message.Text}");
            handler.OnHandleUpdateCompleted += (Message message) => Console.WriteLine($"Закончилась обработка сообщения {message.Text}");
            
            try
            {
               
                
                botClient.StartReceiving(handler, receiverOptions, cancellationToken: cts.Token);

                var me = await botClient.GetMe(cancellationToken: cts.Token);
                Console.WriteLine($"{me.FirstName} запущен!");
                Console.WriteLine("Нажмите клавишу A для выхода");
                while (!cts.IsCancellationRequested)
                {
                    if (Console.ReadKey().Key == ConsoleKey.A)
                    {
                        cts.Cancel();
                    }
                    else
                    {
                        Console.WriteLine($"{me.FirstName} продолжает работать!");
                    }
                    await Task.Delay(1, cancellationToken: cts.Token);
                }
            }
            finally
            {
                cts.Cancel();
                handler.OnHandleUpdateStarted -= (Message message) => Console.WriteLine($"Началась обработка сообщения {message.Text}");
                handler.OnHandleUpdateCompleted -= (Message message) => Console.WriteLine($"Закончилась обработка сообщения {message.Text}");
            }
        }
    }
}
