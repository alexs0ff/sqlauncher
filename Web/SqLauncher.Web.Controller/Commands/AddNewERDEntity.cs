// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AddNewERDEntity.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 05 3:08 PM
//   * Modified at: 2011  09 05  5:39 PM
// / ******************************************************************************/ 

using System.Windows;

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Adds a new ERD Entity.
    /// </summary>
    public class AddNewERDEntity : ICommand
    {
        /// <summary>
        ///   The data model.
        /// </summary>
        public DataModel DataModel { get; set; }

        /// <summary>
        ///   The added entity.
        /// </summary>
        public ERDEntity Entity { get; set; }

        /// <summary>
        ///   The Added entity from.
        /// </summary>
        public IEntityViewState EntityForm { get; set; }

        /// <summary>
        ///   Executes a command.
        /// </summary>
        public void Do()
        {
            DataModel.Entities.Add( Entity );
            Controller.CreateEntityFormByViewState( EntityForm );
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            DataModel.Entities.Remove( Entity );
            Controller.RemoveEntityForm( EntityForm );
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        ///   The current model controller.
        /// </summary>
        public ModelController Controller { get; set; }
    }
}