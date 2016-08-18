// <copyright file="MidProxyService.cs" company="LogicalisSMC">
//     LogicalisSMC. All rights reserved.
// </copyright>
// <author>Bjorn van Dommelen</author>
namespace MidProxy
{
    using System.Diagnostics;
    using System.ServiceProcess;
    using System.Threading;

    /// <summary>
    /// The service container for the MidProxy solution.
    /// </summary>
    public partial class MidProxyService : ServiceBase
    {
        /// <summary>
        /// Flag to indicate whether the service main function should exist as soon as possible.
        /// </summary>
        private bool isStopRequested = false;

        /// <summary>
        /// The main worker thread for this service.
        /// </summary>
        private Thread serviceWorker;

        /// <summary>
        /// Initializes a new instance of the <see cref="MidProxyService" /> class.
        /// </summary>
        public MidProxyService()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Method to start the service in a debug scenario.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public void StartDebugging(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());
            Debug.WriteLine("Debug: started service MidProxyService as command line application");
            this.OnStart(args);
        }

        /// <summary>
        /// Method to stop the service in a debug scenario.
        /// </summary>
        public void StopDebugging()
        {
            Debug.WriteLine("Debug: stopping service MidProxyService as command line application");
            this.OnStop();
            this.serviceWorker.Join();
        }

        /// <summary>
        /// This method is called by the Service Control Manager when the service is started.
        /// </summary>
        /// <param name="args">Command line arguments or service arguments</param>
        protected override void OnStart(string[] args)
        {
            this.serviceWorker = new Thread(new ThreadStart(() => this.ServiceMain(args)));
            this.serviceWorker.Start();
        }

        /// <summary>
        /// This method is called by the Service Control Manager when the service is stopped.
        /// </summary>
        protected override void OnStop()
        {
            this.isStopRequested = true;
        }

        /// <summary>
        /// This is the main work method which gets started as a separate thread.
        /// </summary>
        /// <param name="args">Command line arguments or service arguments</param>
        private void ServiceMain(string[] args)
        {
            while (!this.isStopRequested)
            {
                Debug.WriteLine("Args: " + string.Join(",", args));
                Thread.Sleep(5000);
            }
        }
    }
}
