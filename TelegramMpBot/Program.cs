using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramMpBot.Database;

namespace TelegramMpBot
{
    class Program
    {
        private const string TOKEN  = "5081642403:AAFVRhGPCIw8ADl3Fa1LzIX0sa4qIUUxgPU";
        private static TelegramBotClient BotClient;
        static void Main(string[] args)
        {
            try
            {
                BotClient = new TelegramBotClient(TOKEN);
                BotClient.StartReceiving();
                BotClient.OnMessage += BotClient_OnMessage;
                Console.ReadLine();
                BotClient.StopReceiving();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }
           
        }

        private static async void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var msg = e.Message;
            if (string.IsNullOrEmpty(msg.Text)) return;
            Console.WriteLine(msg.Text);
            if (msg.Text.Equals("Уведомления", StringComparison.OrdinalIgnoreCase))
            {
                using (DatabaseConnection db = new DatabaseConnection())
                {
                    var list = db.Violators.Where(o => !o.IsWatched).ToList();
                    if (list.Count == 0)
                    {
                        await BotClient.SendTextMessageAsync(e.Message.Chat.Id, "Пока нет нарушителей. Но это только пока=)", replyMarkup: GetButtons());
                    }
                    else
                    {
                        string report = string.Empty;
                        int counter = 0;
                        foreach (var violator in list)
                        {
                            report += $"Имя : {violator.Name}   Бренд : {violator.Brand}   Товар : {violator.Product}  \n";
                            violator.IsWatched = true;
                            db.Update(violator);
                            if (counter % 12 ==0)
                            {
                                try
                                {
                                    await BotClient.SendTextMessageAsync(e.Message.Chat.Id, report,
                                        replyMarkup: GetButtons(),cancellationToken: new CancellationTokenSource(2000).Token);
                                }
                                catch
                                {
                                    await Task.Delay(6666);
                                }
                            }
                        }
                     
                        await db.SaveChangesAsync();
                    }
                }
            }
            else
            {
                await BotClient.SendTextMessageAsync(e.Message.Chat.Id,"Добро пожаловать", replyMarkup: GetButtons());
            }
        }
        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text="Уведомления"} }
                }
            };
        }
    }
}
