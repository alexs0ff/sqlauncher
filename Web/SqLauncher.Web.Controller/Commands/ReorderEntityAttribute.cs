// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ReorderEntityAttribute.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 03 06 22:24
//   * Modified at: 2012  03 06  22:31
// / ******************************************************************************/ 

using System.Collections.Generic;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Reorders attributes within entity.
    /// </summary>
    public class ReorderEntityAttribute : ICommand
    {
        /// <summary>
        ///   The entity that hosts the attribute.
        /// </summary>
        public ERDEntity Entity { get; set; }

        /// <summary>
        ///   Gets or sets the old index of attribute.
        /// </summary>
        public int OldIndex { get; set; }

        /// <summary>
        ///   Gets or sets the new index of attribute.
        /// </summary>
        public int NewIndex { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            var list = (IList<EntityAttribute>) Entity.Attributes;
            
            var attribute = list[OldIndex];
            list.RemoveAt( OldIndex );
            list.Insert(NewIndex, attribute);
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            var list = (IList<EntityAttribute>) Entity.Attributes;
            var attribute = list[NewIndex];
            list.RemoveAt( NewIndex );
            list.Insert(OldIndex, attribute);
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}