// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ScaledMouseMoveEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 29 10:27 PM
//   * Modified at: 2011  09 29  10:29 PM
// / ******************************************************************************/ 

using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Mouse move event args of mouse move event with additional information of current container scale.
    /// </summary>
    public class ScaledMouseMoveEventArgs : MouseMoveEventArgs
    {
        /// <summary>
        ///   The scale by the X-axis.
        /// </summary>
        public double ScaleX { get; private set; }

        /// <summary>
        ///   The scale by the Y-Axis.
        /// </summary>
        public double ScaleY { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.MouseMoveEventArgs" /> class.
        /// </summary>
        public ScaledMouseMoveEventArgs( double scaleX, double scaleY, Point position ) : base( position )
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
        }
    }
}