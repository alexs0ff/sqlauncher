// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AddNewEntityIndex.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 02 23 19:56
//   * Modified at: 2012  02 25  19:09
// / ******************************************************************************/ 

using SqLauncher.Web.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Adds new Entity index to ERDEntity.
    /// </summary>
    public class AddNewEntityIndex : ICommand
    {
        /// <summary>
        ///   The erd entity.
        /// </summary>
        public ERDEntity ERDEntity { get; set; }

        /// <summary>
        ///   The added entity index.
        /// </summary>
        public EntityIndex Index { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            Index.Parent = ERDEntity;
            ERDEntity.Indexes.Add( Index );
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            Index.Parent = null;
            ERDEntity.Indexes.Remove( Index );
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}