// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DeserializingEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 01 08 12:49
//   * Modified at: 2012  01 08  12:53
// / ******************************************************************************/ 

using System;
using System.IO;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The event args for deserializing event.
    /// </summary>
    public class DeserializingEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.DeserializingEventArgs" /> class.
        /// </summary>
        public DeserializingEventArgs( string fileName, Stream stream )
        {
            FileName = fileName;
            Stream = stream;
        }

        /// <summary>
        ///   The file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///   The data stream.
        /// </summary>
        public Stream Stream { get; set; }
    }
}