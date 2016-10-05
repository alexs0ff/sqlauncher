// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   BrushChangedEventArgs.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2012 03 02 21:12
//   * Modified at: 2012  03 02  21:16
// / ******************************************************************************/ 

using System;
using System.Windows.Media;

namespace SqLauncher.Web.Ribbon
{
    /// <summary>
    ///   The event args of brush color changed event.
    /// </summary>
    public class BrushChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Ribbon.BrushChangedEventArgs" /> class.
        /// </summary>
        public BrushChangedEventArgs( Brush brush, Brush oldBrush )
        {
            Brush = brush;
            OldBrush = oldBrush;
        }

        /// <summary>
        ///   The new brush.
        /// </summary>
        public Brush Brush { get; private set; }

        /// <summary>
        ///   The old brush.
        /// </summary>
        public Brush OldBrush { get; private set; }
    }
}