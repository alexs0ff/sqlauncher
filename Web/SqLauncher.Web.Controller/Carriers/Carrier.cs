// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   Carrier.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 10 04 10:04 PM
//   * Modified at: 2011  10 04  10:07 PM
// / ******************************************************************************/ 

using System.Windows;

namespace SqLauncher.Web.Controller.Carriers
{
    /// <summary>
    ///   The mouse move helper.
    /// </summary>
    public abstract class Carrier
    {
        /// <summary>
        /// Starts the moving.
        /// </summary>
        /// <param name="point">The start point.</param>
        public abstract void Start( Point point );

        /// <summary>
        /// Stops the moving.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        ///   Moves to next point.
        /// </summary>
        /// <param name = "newPoint">The destination.</param>
        public abstract void Move( Point newPoint );

        /// <summary>
        ///   The start porting point.
        /// </summary>
        public Point StartPoint { get;protected set; }

        /// <summary>
        ///   The end porting point.
        /// </summary>
        public Point EndPoint { get; protected set; }
    }
}