// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityFormChangePosition.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 05 6:18 PM
//   * Modified at: 2011  09 12  8:22 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Change a current position.
    /// </summary>
    public class EntityFormChangePosition : ICommand
    {
        /// <summary>
        ///   The new position.
        /// </summary>
        public Point NewPosition { get; set; }

        /// <summary>
        ///   The old position.
        /// </summary>
        public Point OldPosition { get; set; }

        /// <summary>
        ///   The handled entity form id.
        /// </summary>
        public Guid EntityFormId { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            ViewManager.Move( EntityFormId, NewPosition );
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            ViewManager.Move(EntityFormId, OldPosition);
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// The model view manager.
        /// </summary>
        public ModelViewManager ViewManager { get; set; }
    }
}