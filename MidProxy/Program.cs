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
        /// The resource manager to retrieve console messages from.
        /// </summary>
        private static ResourceManager messages = new ResourceManager("Messages", typeof(Program).Assembly);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command line or service arguments</param>
        public static void Main(string[] args)
        {
            if (args.Contains("-debug"))
            {
                using (MidProxyService midProxy = new MidProxyService())
                {
                    midProxy.StartDebugging(args);
                    Console.Read();
                    midProxy.StopDebugging();
                }
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
                ServiceBase[] servicesToRun = new ServiceBase[]
                {
                    new MidProxyService()
                };
                ServiceBase.Run(servicesToRun);
            }
        }

        /// <summary>
        /// Installs the service on the system.
        /// </summary>
        /// <param name="args">Installation command line arguments</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Swallow the rollback exception and let the real cause bubble up.")]
        internal static void Install(string[] args)
        {
            try
            {
                Console.WriteLine(messages.GetString("SVC_INSTALL"));
                using (AssemblyInstaller inst =
                    new AssemblyInstaller(typeof(Program).Assembly, args))
                {
                    IDictionary state = new Hashtable();
                    inst.UseNewContext = true;
                    try
                    {
                        inst.Install(state);
                        inst.Commit(state);
                        Console.WriteLine(messages.GetString("SVC_INSTALL_OK"));
                    }
                    catch
                    {
                        try
                        {
                            Console.WriteLine(messages.GetString("SVC_INSTALL_NOK"));
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
        internal static void Uninstall(string[] args)
        {
            try
            {
                Console.WriteLine(messages.GetString("SVC_UNINSTALL"));
                using (AssemblyInstaller inst =
                    new AssemblyInstaller(typeof(Program).Assembly, args))
                {
                    IDictionary state = new Hashtable();
                    inst.UseNewContext = true;
                    try
                    {
                        inst.Uninstall(state);
                        Console.WriteLine(messages.GetString("SVC_UNINSTALL_OK"));
                    }
                    catch
                    {
                        try
                        {
                            Console.WriteLine(messages.GetString("SVC_UNINSTALL_NOK"));
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
