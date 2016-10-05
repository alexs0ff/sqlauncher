// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   LayoutZManager.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 02 15 20:34
//   * Modified at: 2012  02 15  23:22
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   The manager for layout engine of z axis.
    /// </summary>
    internal class LayoutZManager
    {
        #region Z indexes 

        /// <summary>
        ///   The default z index of all relation forms.
        /// </summary>
        private const int RelationFormZIndex = 1;

        /// <summary>
        ///   The dafault z index of relation form edit window.
        /// </summary>
        private const int RelationFormEditIndex = 5;

        /// <summary>
        ///   The default z index of all entity forms.
        /// </summary>
        private const int EntityFormZIndex = 1;

        /// <summary>
        ///   The default z index of all selected entity forms.
        /// </summary>
        private const int SelectedEntityFormZIndex = 3;

        /// <summary>
        ///   The dafault z index of entity form edit window.
        /// </summary>
        private const int EntityFormEditIndex = 5;

        /// <summary>
        ///   The last user selected entity form.
        /// </summary>
        private const int LastPickedEditFormZIndex = 9;

        #endregion Z indexes

        #region Relation form handling 

        /// <summary>
        ///   The registred relation forms.
        /// </summary>
        private readonly IDictionary<Guid, RelationForm> _relationForms = new Dictionary<Guid, RelationForm>();

        /// <summary>
        ///   Registres the relation form.
        /// </summary>
        /// <param name = "relationForm">The relation form.</param>
        public void Register( RelationForm relationForm )
        {
            _relationForms.Add( relationForm.DataEntity.Relation.InnerId, relationForm );
            relationForm.RelationEdit.AppearanceChanged += RelationEditAppearanceChanged;
            relationForm.RelationEdit.MouseLeftButtonDown += RelationEditMouseLeftButtonDown;

            Canvas.SetZIndex( relationForm, RelationFormZIndex );
            Canvas.SetZIndex( relationForm.RelationEdit, RelationFormEditIndex );
        }

        /// <summary>
        ///   Unregistres the relation form.
        /// </summary>
        /// <param name = "relationForm">The relation form.</param>
        public void Unregister( RelationForm relationForm )
        {
            relationForm.RelationEdit.AppearanceChanged -= RelationEditAppearanceChanged;
            relationForm.RelationEdit.MouseLeftButtonDown -= RelationEditMouseLeftButtonDown;
            _relationForms.Remove( relationForm.DataEntity.Relation.InnerId );
        }

        /// <summary>
        ///   Occurs when user mouse left button down on relation form edit.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event arg.</param>
        private void RelationEditMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            ProcessRelationFormEditSelect( (RelationFormEdit) sender );
        }

        /// <summary>
        ///   The last selected form edit.
        /// </summary>
        private UserControl _lastFormEdit;

        /// <summary>
        ///   Proceses the relation form edit selection.
        /// </summary>
        /// <param name = "relationFormEdit">The selected relation form edit.</param>
        private void ProcessRelationFormEditSelect( RelationFormEdit relationFormEdit )
        {
            if ( _lastFormEdit != null ){
                Canvas.SetZIndex( _lastFormEdit, RelationFormEditIndex );
            } //if

            _lastFormEdit = relationFormEdit;
            Canvas.SetZIndex( _lastFormEdit, LastPickedEditFormZIndex );
        }

        /// <summary>
        ///   Occurs when relation edit form has been appeared.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void RelationEditAppearanceChanged( object sender, AppearanceChangedEventArgs e )
        {
            ProcessRelationFormEditApearenceChanged( (RelationFormEdit) sender, e.Appeared );
        }

        /// <summary>
        ///   Proceees the all appearence changes of relation form.
        /// </summary>
        /// <param name = "relationFormEdit">The relation form edit.</param>
        /// <param name = "appeared">The appeared flag.</param>
        private void ProcessRelationFormEditApearenceChanged( RelationFormEdit relationFormEdit, bool appeared )
        {
            if ( appeared ){
                ProcessRelationFormEditSelect( relationFormEdit );
            } //if
            else{
                Canvas.SetZIndex( relationFormEdit, RelationFormEditIndex );
            } //else
        }

        #endregion Relation form handling

        #region Entity form handling

        /// <summary>
        ///   The registred erd entity forms.
        /// </summary>
        private readonly IDictionary<Guid, EntityForm> _registredEntityForms = new Dictionary<Guid, EntityForm>();

        /// <summary>
        ///   Registres the entity form for watch.
        /// </summary>
        /// <param name = "entityForm">The entity form.</param>
        public void Register( EntityForm entityForm )
        {
            _registredEntityForms.Add( entityForm.DataEntity.Entity.InnerId, entityForm );
            entityForm.SelectionStateChanged += EntityFormSelectionStateChanged;
            entityForm.GetEditForm().MouseLeftButtonDown += EditFormMouseLeftButtonDown;
            entityForm.GetEditForm().AppearanceChanged += EntityFormEditAppearenceChanged;

            Canvas.SetZIndex( entityForm, EntityFormZIndex );
            Canvas.SetZIndex( entityForm.GetEditForm(), EntityFormEditIndex );
        }

        /// <summary>
        ///   Unregistres the entity form.
        /// </summary>
        /// <param name = "entityForm">The entity form.</param>
        public void Unregister( EntityForm entityForm )
        {
            entityForm.SelectionStateChanged -= EntityFormSelectionStateChanged;
            entityForm.GetEditForm().MouseLeftButtonDown -= EditFormMouseLeftButtonDown;
            entityForm.GetEditForm().AppearanceChanged -= EntityFormEditAppearenceChanged;

            _registredEntityForms.Remove( entityForm.DataEntity.Entity.InnerId );
        }

        private void EditFormMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            ProcessEditFormSelect( (EntityFormEdit) sender );
        }

        /// <summary>
        ///   Occurs when selection state has been changed
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void EntityFormSelectionStateChanged( object sender, SelectionStateChangedEventArgs e )
        {
            ProcessChangeZIndex( (EntityForm) sender );
        }

        /// <summary>
        ///   Occurs when appearence of entity form edit has been changed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void EntityFormEditAppearenceChanged( object sender, AppearanceChangedEventArgs e )
        {
            ProcessEntityFormEditApearenceChanged( (EntityFormEdit) sender, e.Appeared );
        }

        /// <summary>
        ///   Processes all appearence changes of entity form edit.
        /// </summary>
        /// <param name = "entityFormEdit">The entity form edit.</param>
        /// <param name = "appeared">The appearad flag.</param>
        private void ProcessEntityFormEditApearenceChanged( EntityFormEdit entityFormEdit, bool appeared )
        {
            if ( appeared ){
                ProcessEditFormSelect( entityFormEdit );
            } //if
            else{
                Canvas.SetZIndex( entityFormEdit, EntityFormEditIndex );
            } //else
        }

        /// <summary>
        ///   Processes the edit form select.
        /// </summary>
        /// <param name = "entityFormEdit">The entity form edit.</param>
        private void ProcessEditFormSelect( EntityFormEdit entityFormEdit )
        {
            if ( _lastFormEdit != null ){
                Canvas.SetZIndex( _lastFormEdit, EntityFormEditIndex );
            } //if
            _lastFormEdit = entityFormEdit;

            Canvas.SetZIndex( _lastFormEdit, LastPickedEditFormZIndex );
        }

        /// <summary>
        ///   Processes the z index changing.
        /// </summary>
        private void ProcessChangeZIndex( EntityForm entityForm )
        {
            if ( entityForm.IsSelected ){
                Canvas.SetZIndex( entityForm, SelectedEntityFormZIndex );
            } //if
            else{
                Canvas.SetZIndex( entityForm, EntityFormZIndex );
            } //else
        }

        #endregion Entity form handling
    }
}