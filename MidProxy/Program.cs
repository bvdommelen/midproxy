// <copyright file="Program.cs" company="LogicalisSMC">
//     LogicalisSMC. All rights reserved.
// </copyright>
// <author>Bjorn van Dommelen</author>
[assembly: System.CLSCompliant(true)]

namespace MidProxy
{
    using System;
    using System.Collections;
    using System.Configuration.Install;
    using System.Linq;
    using System.Resources;
    using System.ServiceProcess;

    /// <summary>
    /// The entry point for the service and command line application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command line or service arguments</param>
        public static void Main(string[] args)
        {
            if (args.Contains("-debug"))
            {
                Debug(args);
            }
            else if (args.Contains("-install"))
            {
                Install(args);
            }
            else if (args.Contains("-uninstall"))
            {
                Uninstall(args);
            }
            else
            {
                RunAsService();
            }
        }

        /// <summary>
        /// Debug the program as a command line application.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        private static void Debug(string[] args)
        {
            using (MidProxyService midProxy = new MidProxyService())
            {
                midProxy.StartDebugging(args);
                Console.Read();
                midProxy.StopDebugging();
            }
        }

        /// <summary>
        /// Runs the program as a service.
        /// </summary>
        private static void RunAsService()
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                    new MidProxyService()
            };
            ServiceBase.Run(servicesToRun);
        }

        /// <summary>
        /// Installs the service on the system.
        /// </summary>
        /// <param name="args">Installation command line arguments</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Swallow the rollback exception and let the real cause bubble up.")]
        private static void Install(string[] args)
        {
            try
            {
                Console.WriteLine(MidProxy.Messages.SVC_INSTALL);
                using (AssemblyInstaller inst =
                    new AssemblyInstaller(typeof(Program).Assembly, args))
                {
                    IDictionary state = new Hashtable();
                    inst.UseNewContext = true;
                    try
                    {
                        inst.Install(state);
                        inst.Commit(state);
                        Console.WriteLine(MidProxy.Messages.SVC_INSTALL_OK);
                    }
                    catch
                    {
                        try
                        {
                            Console.WriteLine(MidProxy.Messages.SVC_INSTALL_NOK);
                            inst.Rollback(state);
                        }
                        catch
                        {
                            // Swallow the rollback exception
                            // and let the real cause bubble up.
                        }

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Uninstalls the service from the system.
        /// </summary>
        /// <param name="args">Uninstall command line arguments</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Swallow the rollback exception and let the real cause bubble up.")]
        private static void Uninstall(string[] args)
        {
            try
            {
                Console.WriteLine(MidProxy.Messages.SVC_UNINSTALL);
                using (AssemblyInstaller inst =
                    new AssemblyInstaller(typeof(Program).Assembly, args))
                {
                    IDictionary state = new Hashtable();
                    inst.UseNewContext = true;
                    try
                    {
                        inst.Uninstall(state);
                        Console.WriteLine(MidProxy.Messages.SVC_UNINSTALL_OK);
                    }
                    catch
                    {
                        try
                        {
                            Console.WriteLine(MidProxy.Messages.SVC_UNINSTALL_NOK);
                            inst.Rollback(state);
                        }
                        catch
                        {
                            // Swallow the rollback exception
                            // and let the real cause bubble up.
                        }

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
