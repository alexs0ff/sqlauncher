// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   BrushChangedEventArgs.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 10 02 4:29 PM
//   * Modified at: 2011  10 02  4:31 PM
// / ******************************************************************************/ 

using System;
using System.Windows.Media;

namespace SqLauncher.Web.Controller.RibbonIteraction
{
    /// <summary>
    ///   Occurs when user changed a brush.
    /// </summary>
    public class BrushChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   The brush.
        /// </summary>
        public Brush Brush { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.RibbonIteraction.BrushChangedEventArgs" /> class.
        /// </summary>
        public BrushChangedEventArgs( Brush brush )
        {
            Brush = brush;
        }
    }
}