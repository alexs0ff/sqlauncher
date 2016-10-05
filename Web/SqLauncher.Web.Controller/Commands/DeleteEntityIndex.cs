// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DeleteEntityIndex.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 02 25 19:13
//   * Modified at: 2012  02 25  19:14
// / ******************************************************************************/ 

using SqLauncher.Web.Model;

namespace SqLauncher.Web.Controller.Commands
{
    public class DeleteEntityIndex : ICommand
    {
        /// <summary>
        ///   The erd entity.
        /// </summary>
        public ERDEntity ERDEntity { get; set; }

        /// <summary>
        ///   The deleted entity index.
        /// </summary>
        public EntityIndex Index { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            ERDEntity.Indexes.Remove( Index );
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            ERDEntity.Indexes.Add( Index );
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}