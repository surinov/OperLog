using System;
using System.Threading;

namespace OperLog
{
    class Program
    {
        static void Main(String[] args) {
            Sender s = new Sender();
            s.Main();
            Log l = new Log();
            l.Main();
            while (true) { Thread.Sleep(1);}
        }
    }
}
