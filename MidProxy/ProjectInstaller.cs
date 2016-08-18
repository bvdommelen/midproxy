// <copyright file="ProjectInstaller.cs" company="LogicalisSMC">
//     LogicalisSMC. All rights reserved.
// </copyright>
// <author>Bjorn van Dommelen</author>
namespace MidProxy
{
    using System.ComponentModel;
    
    /// <summary>
    /// Installer for the MidProxy project.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInstaller" /> class.
        /// </summary>
        public ProjectInstaller()
        {
            this.InitializeComponent();
        }
    }
}
