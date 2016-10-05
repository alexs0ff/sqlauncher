// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   VersionViewLoadedEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 02 04 15:25
//   * Modified at: 2012  02 04  15:26
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The evnt args for version view loaded event.
    /// </summary>
    public class VersionViewLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.UI.Model.VersionViewLoadedEventArgs"/> class.
        /// </summary>
        public VersionViewLoadedEventArgs( DatabaseVersion databaseVersion )
        {
            DatabaseVersion = databaseVersion;
        }

        /// <summary>
        /// The removed database version.
        /// </summary>
        public DatabaseVersion DatabaseVersion { get; private set; }
    }
}