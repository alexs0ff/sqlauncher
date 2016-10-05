// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ModelControllerHandler.cs
//   * Project: SqLauncher.Web.Designer
//   * Description:
//   * Created at 2011 09 26 9:02 PM
//   * Modified at: 2011  09 26  9:12 PM
// / ******************************************************************************/ 

using SqLauncher.Web.Controller;
using SqLauncher.Web.Model;

namespace SqLauncher.Web.Designer
{
    /// <summary>
    ///   Represents the handler for delegation methods of ModelController class
    /// </summary>
    public class ModelControllerHandler
    {
        /// <summary>
        ///   The handled model view controller.
        /// </summary>
        private readonly ModelController _handledController;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Designer.ModelControllerHandler" /> class.
        /// </summary>
        public ModelControllerHandler( IViewController handledController )
        {
            _handledController = handledController as ModelController;
        }

        /// <summary>
        ///   Undoes the last command.
        /// </summary>
        public void Undo()
        {
            if ( _handledController == null ){
                return;
            } //if

            _handledController.Undo();
        }

        /// <summary>
        ///   Redoes the undues commands.
        /// </summary>
        public void Redo()
        {
            if ( _handledController == null ){
                return;
            } //if

            _handledController.Redo();
        }

        /// <summary>
        ///   Creates a new entity from and admit user to place it.
        /// </summary>
        public void CreateAndPlaceEntityForm()
        {
            if ( _handledController == null ){
                return;
            } //if

            _handledController.CreateAndPlaceEntityForm();
        }

        /// <summary>
        /// Creates a new relation from and admit user to place it.
        /// </summary>
        /// <param name="relationshipType">The realtionship type.</param>
        public void CreateAndPlaceRelationForm(RelationshipType relationshipType)
        {
            if ( _handledController == null ){
                return;
            } //if

            _handledController.CreateAndPlaceRelationForm(relationshipType);
        }

        /// <summary>
        /// Gets the iteraction provider of handled controller.
        /// If handled controller null then reterns UserIteractionProvider.Default instance.
        /// </summary>
        /// <returns>The iteraction provider.</returns>
        public UserIteractionProvider GetIteractionProvider()
        {
            if ( _handledController == null ){
                return UserIteractionProvider.Default;    
            } //if

            return _handledController.IteractionProvider;
        }

        /// <summary>
        /// Starts generate ddl script.
        /// </summary>
        public void StartGeneratingSql()
        {
            if (_handledController == null)
            {
                return;
            } //if

            _handledController.StartGeneratingSql();
        }

        /// <summary>
        /// Copies selected items into clipboard.
        /// </summary>
        public void Copy()
        {
            if (_handledController == null)
            {
                return;
            } //if

            _handledController.ClipboardCopy();
        }

        /// <summary>
        /// Paste items from clipboard.
        /// </summary>
        public void Paste()
        {
            if (_handledController == null)
            {
                return;
            } //if

            _handledController.ClipboardPaste();
        }

        /// <summary>
        /// Paste items from clipboard.
        /// </summary>
        public void Duplicate()
        {
            if (_handledController == null)
            {
                return;
            } //if

            _handledController.DuplicateSelectedItems();
        }

        /// <summary>
        /// Cuts the selected item to clipboard.
        /// </summary>
        public void Cut()
        {
            if ( _handledController==null ){
                return;
            } //if

            _handledController.ClipboardCut();
        }

        /// <summary>
        /// Canceles a current operation.
        /// </summary>
        public void CancelCurrentOperation()
        {
            if (_handledController == null)
            {
                return;
            } //if

            _handledController.CancelCurrentOperation();
        }

        /// <summary>
        /// Removes all selected items.
        /// </summary>
        public void RemoveSelectedItems()
        {
            if (_handledController == null)
            {
                return;
            } //if

            _handledController.RemoveSelectedItems();
        }

        /// <summary>
        /// Register change command for all cashed entity form background changes.
        /// </summary>
        public void RegisterEntityFormsBackgroundBrushChange()
        {
            if (_handledController == null)
            {
                return;
            } //if

            _handledController.RegisterEntityFormsBackgroundBrushChange();
        }
    }
}