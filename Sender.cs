using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace OperLog
{
    class Sender
    {
        BackgroundWorker bw;
        TelegramBotClient Bot = new Telegram.Bot.TelegramBotClient("1390500172:AAGZpceToZXEFcj1cAu7emYqp9APgKroxBw");
        public void main()
        {
            this.bw = new BackgroundWorker();
            this.bw.DoWork += bw_DoWork;
            this.bw.RunWorkerAsync();
        }
           
        async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                await Bot.SetWebhookAsync(""); 


                Bot.OnUpdate += async (object su, Telegram.Bot.Args.UpdateEventArgs evu) =>
                {
                    if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null) return; // в этом блоке нам келлбэки и инлайны не нужны
                    var update = evu.Update;
                    var message = update.Message;
                    if (message == null) return;
                    if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                    {
                        if (message.Text == "/say")
                        {
                            SendMessage(message.Chat.Id, "test");
                        }
                        if (message.Text == "/myid")
                        {
                            //await Bot.SendPhotoAsync(message.Chat.Id, "http://aftamat4ik.ru/wp-content/uploads/2017/03/photo_2016-12-13_23-21-07.jpg", "Revolution!");
                            SendMessage(message.Chat.Id, message.Chat.Id.ToString());
                        }
                    }
                };

                // запускаем прием обновлений
                Bot.StartReceiving();
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message); // если ключ не подошел - пишем об этом в консоль отладки
            }

        }

        async void SendMessage(long id, string text)
        {
            await Bot.SendTextMessageAsync(id, text);
        }

        public async void SendLog(string text)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            const string screen = @"C:\Users\surinchik\Documents\GitHub\OperLog\bin\Debug\icon.png";
            using (var fileStream = new FileStream(screen, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await Bot.SendPhotoAsync(289675402, new InputOnlineFile(fileStream),text);
            }
        }
    }
}
