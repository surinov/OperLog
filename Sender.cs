using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace OperLog
{
    class Sender
    {
        BackgroundWorker _bw;
        TelegramBotClient Bot = new Telegram.Bot.TelegramBotClient("1390500172:AAGZpceToZXEFcj1cAu7emYqp9APgKroxBw");
        public void Main() {
            this._bw = new BackgroundWorker();
            this._bw.DoWork += Bot_Tg;
            this._bw.RunWorkerAsync();
        }
        //TODO: проверять есть ли интернет-соединение, чтобы не стопить прогу
        async void Bot_Tg(object sender, DoWorkEventArgs e) {
            var worker = sender as BackgroundWorker;
            try {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                await Bot.SetWebhookAsync(""); 
                Bot.OnUpdate += async (object su, Telegram.Bot.Args.UpdateEventArgs evu) => {
                    if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null) return;
                    var update = evu.Update;
                    var message = update.Message;
                    if (message == null) return;
                    if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text) {
                        if (message.Text == "/state") { 
                            SendMessage(message.Chat.Id, "State: in progress");
                        }
                        if (message.Text == "/myid") { 
                            SendMessage(message.Chat.Id, message.Chat.Id.ToString());
                        }
                    }
                };
                Bot.StartReceiving();
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) {
                Console.WriteLine(ex.Message);
            }
        }

        async void SendMessage(long id, string text) {
            await Bot.SendTextMessageAsync(id, text);
        }

        public async void SendLog(string text,string path) {
            try {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls;
                try {
                    using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        await Bot.SendPhotoAsync(289675402, new InputOnlineFile(fileStream), text);
                    }
                }
                catch {
                    await Bot.SendTextMessageAsync(289675402, text);
                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) {
                SendMessage(289675402, "Exeption!!" + ex.ToString());
            }
        }
    }
}
