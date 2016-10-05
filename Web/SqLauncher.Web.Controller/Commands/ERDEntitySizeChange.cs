// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ERDEntitySizeChange.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  04 20  23:31
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Change the size of the entity.
    /// </summary>
    public class ERDEntitySizeChange : ICommand
    {
        /// <summary>
        ///   The old size.
        /// </summary>
        public Rect OldSize { get; set; }

        /// <summary>
        ///   The new size.
        /// </summary>
        public Rect NewSize { get; set; }

        /// <summary>
        ///   The handled entity form id.
        /// </summary>
        public Guid EntityFormId { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            ViewManager.SetFormSize( EntityFormId, NewSize );
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            ViewManager.SetFormSize( EntityFormId, OldSize );
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        ///   The model view manager.
        /// </summary>
        public ModelViewManager ViewManager { get; set; }
    }
}