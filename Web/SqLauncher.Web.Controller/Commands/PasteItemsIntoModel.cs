using System;
using System.Linq;
using System.Collections.Generic;

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    /// Insertes an items to model.
    /// </summary>
    public class PasteItemsIntoModel:ICommand
    {
        /// <summary>
        /// The erd entities.
        /// </summary>
        private ICollection<IEntityViewState> _entities;

        /// <summary>
        /// The erd entities.
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
        ///   The current model controller.
        /// </summary>
        public ModelController Controller { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            foreach ( var entityViewState in Entities ){
                Controller.CreateEntityFormByViewState( entityViewState );
            } //foreach
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            foreach ( var entityViewState in Entities ){
                Controller.RemoveEntityForm( entityViewState );
            } //foreach
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}
