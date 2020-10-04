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
        public const int AdminId = 289675402;
        public bool SetMode = false;
        public bool SetParam = false;
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
                    if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text & SetMode == false & SetParam == false)
                    {
                        switch (message.Text)
                        {
                            case "/state":
                                SendMessage(message.Chat.Id, "State: in progress");
                                break;
                            case "/settings":
                                SendMessage(message.Chat.Id, "Введите номер параметра\n" +
                                                             "1. Частота отправки лога" +
                                                             "\n2. Выйти из режима");
                                SetMode = true;
                                break;
                            case "/myid":
                                SendMessage(message.Chat.Id, message.Chat.Id.ToString());
                                break;
                        }
                    }
                    else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text & SetMode == true & SetParam == false)
                    {
                        if (message.Text.Contains("1") || message.Text.Contains("1."))
                        {
                            SendMessage(message.Chat.Id, "Введите значение");
                            SetParam = true;
                        }
                        else if (message.Text == "2")
                        {
                            SetMode = false;
                            SendMessage(message.Chat.Id, "Режим настройки отключен");
                        }
                    }
                    else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text & SetMode == true & SetParam == true)
                    {
                        int.TryParse(message.Text, out int param);
                        SendMessage(message.Chat.Id, "Значение " + param + " установлено");
                        SetParam = false;
                        SetMode = false;
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

        public async void SendLog(string text,string picpath) {
            try {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls;
                try {
                    using (var fileStream = new FileStream(picpath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        await Bot.SendPhotoAsync(AdminId, new InputOnlineFile(fileStream), text);
                    }
                }
                catch {
                    await Bot.SendTextMessageAsync(AdminId, "No screen" + text);
                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) {
                SendMessage(289675402, "Exeption!!" + ex.ToString());
            }
        }
    }
}
