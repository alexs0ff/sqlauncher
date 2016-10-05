// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SerializingEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 01 08 12:33
//   * Modified at: 2012  01 08  12:36
// / ******************************************************************************/ 

using System;
using System.IO;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The serializing event args.
    /// </summary>
    public class SerializingEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.SerializingEventArgs" /> class.
        /// </summary>
        public SerializingEventArgs( string fileName , Stream fileStream)
        {
            FileStream = fileStream;
            FileName = fileName;
        }

        /// <summary>
        ///   The file stream.
        /// </summary>
        public Stream FileStream { get; private set; }

        /// <summary>
        ///   The file name.
        /// </summary>
        public string FileName { get; private set; }
    }
}