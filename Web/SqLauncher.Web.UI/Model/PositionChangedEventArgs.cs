// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityFromPositionChangedEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 05 5:42 PM
//   * Modified at: 2011  09 05  5:44 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents the event args of EntityForm position changed event.
    /// </summary>
    public class PositionChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   The old position.
        /// </summary>
        public Point OldPosition { get; set; }


        /// <summary>
        ///   The new position.
        /// </summary>
        public Point NewPosition { get; set; }
    }
}