// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ERDEntityProcessedEventArgs.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 25 15:34
//   * Modified at: 2011  11 25  15:38
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   The event args for ERDEntityProcessed event.
    /// </summary>
    public class ERDEntityProcessedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.ERDEntityProcessedEventArgs" /> class.
        /// </summary>
        public ERDEntityProcessedEventArgs( ERDEntity entity )
        {
            Entity = entity;
        }

        /// <summary>
        ///   The processed entity.
        /// </summary>
        public ERDEntity Entity { get; private set; }
    }
}