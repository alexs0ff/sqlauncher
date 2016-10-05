// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DeleteRelationForms.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 01 17 21:34
//   * Modified at: 2012  01 17  21:46
// / ******************************************************************************/ 

using System.Collections.Generic;

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Removes the relation forms.
    /// </summary>
    public class DeleteRelationForms : ICommand
    {
        /// <summary>
        ///   The current model controller.
        /// </summary>
        public ModelController Controller { get; set; }

        /// <summary>
        ///   The relation forms.
        /// </summary>
        public ICollection<IRelationViewState> RelationViewStates { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            foreach ( var relationViewState in RelationViewStates ){
                Controller.RemoveRelationForm( relationViewState );
            } //foreach
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            foreach ( var relationViewState in RelationViewStates ){
                Controller.CreateRelationFormByViewState( relationViewState );
                Controller.ModelViewManager.RelationLayoutUpdate( relationViewState );
            } //foreach
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}