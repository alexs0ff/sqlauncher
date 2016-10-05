// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqlGeneratingEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 24 20:57
//   * Modified at: 2011  11 24  20:59
// / ******************************************************************************/ 

using System;
using System.IO;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The event args for Sql generating event.
    /// </summary>
    public class SqlGeneratingEventArgs : EventArgs
    {
        /// <summary>
        ///   The stream of a sql file.
        /// </summary>
        private readonly Stream _streamWriter;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.SqlGeneratingEventArgs" /> class.
        /// </summary>
        public SqlGeneratingEventArgs( Stream streamWriter, string filePath )
        {
            _streamWriter = streamWriter;
            FilePath = filePath;
        }

        /// <summary>
        ///    The stream of a sql file.
        /// </summary>
        public Stream StreamWriter
        {
            get { return _streamWriter; }
        }

        /// <summary>
        /// The file path.
        /// </summary>
        public string FilePath { get; private set; }
    }
}