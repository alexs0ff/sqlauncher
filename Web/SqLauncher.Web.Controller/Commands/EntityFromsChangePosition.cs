// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityFromsChangePosition.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  02 16  22:59
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Windows;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Change position for several entity forms.
    /// </summary>
    public class EntityFromsChangePosition : ICommand
    {
        /// <summary>
        ///   The old postions.
        /// </summary>
        public IDictionary<Guid, Point> OldPositions { get; set; }

        /// <summary>
        ///   The new positions.
        /// </summary>
        public IDictionary<Guid, Point> NewPositions { get; set; }

        /// <summary>
        ///   The model view manager.
        /// </summary>
        public ModelViewManager ViewManager { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            foreach ( var newPosition in NewPositions ){
                ViewManager.Move( newPosition.Key, newPosition.Value );
            } //foreach
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            foreach ( var oldPosition in OldPositions ){
                ViewManager.Move( oldPosition.Key, oldPosition.Value );
            } //foreach
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}