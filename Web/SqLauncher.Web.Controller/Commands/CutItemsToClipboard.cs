// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   CutItemsToClipboard.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 02 08 20:30
//   * Modified at: 2012  02 08  20:58
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Linq;

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Cutes items to clipboard.
    /// </summary>
    public class CutItemsToClipboard : ICommand
    {
        /// <summary>
        ///   The current model controller.
        /// </summary>
        public ModelController Controller { get; set; }

        /// <summary>
        ///   The erd entities.
        /// </summary>
        private ICollection<IEntityViewState> _entities;

        /// <summary>
        ///   The erd entities.
        /// </summary>
        public ICollection<IEntityViewState> Entities
        {
            get { return _entities; }
            set
            {
                //create a copy
                _entities = value.ToList();
            }
        }

        /// <summary>
        /// The stored items before cutting.
        /// </summary>
        private ICollection<IEntityViewState> _storedItems;

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            _storedItems = ClipboardManager.Manager.GetStoredEntities();

            ClipboardManager.Manager.SetEntities( Entities );
            foreach ( var entityViewState in Entities ){
                Controller.RemoveEntityForm( entityViewState );
            } //foreach
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            foreach ( var entityViewState in Entities ){
                Controller.CreateEntityFormByViewState( entityViewState );
            } //foreach
            ClipboardManager.Manager.SetEntities( _storedItems );
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}