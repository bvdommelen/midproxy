using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MidProxy
{
    public partial class MidProxy : ServiceBase
    {
        private bool isStopRequested = false;

        public MidProxy()
        {
            InitializeComponent();
        }

        public void DbgStart(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());
            Debug.WriteLine("Debug: started service MidProxy as command line application");
            var t = new Thread(new ThreadStart(() => this.OnStart(args)));
            t.Start();
            Console.Read();
            OnStop();
            t.Join();
        }

        protected override void OnStart(string[] args)
        {
            while (!isStopRequested)
            {
                Debug.WriteLine("Args: " + string.Join(",", args));
                Thread.Sleep(5000);
            }
        }

        protected override void OnStop()
        {
            isStopRequested = true;
        }
    }
}
