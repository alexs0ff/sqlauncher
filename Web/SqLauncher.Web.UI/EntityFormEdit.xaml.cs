// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityFormEdit.xaml.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  02 25  18:52
// / ******************************************************************************/ 

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Behaviors;
using SqLauncher.Web.UI.Common.Shortcuts;
using SqLauncher.Web.UI.Model;
using SqLauncher.Web.UI.Common;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   Represents the edit form fro entity.
    /// </summary>
    public partial class EntityFormEdit
    {
        /// <summary>
        ///   The edited from.
        /// </summary>
        private readonly EntityForm _entityForm;

        public EntityFormEdit( EntityForm entityForm )
        {
            if ( entityForm == null ){
                throw new ArgumentNullException( "entityForm" );
            } //if

            _entityForm = entityForm;
            InitializeComponent();
            attributesDataGrid.MouseLeftButtonDown += DataGridMouseLeftButtonDown;
            indexesDataGrid.MouseLeftButtonDown += DataGridMouseLeftButtonDown;
            GotFocus += ( sender, args ) => ShortcutManager.Manager.StartResistentMode();
            LostFocus += ( sender, args ) => ShortcutManager.Manager.StopResistentMode();
        }

        /// <summary>
        /// The data conext.
        /// </summary>
        private IEntityViewState _dataEntity;

        /// <summary>
        /// The data conext.
        /// </summary>
        public IEntityViewState DataEntity
        {
            get { return _dataEntity; }
            set
            {
                if ( _dataEntity!=null){
                    _dataEntity.Entity.PropertyChanged -= EntityPropertyChanged;
                } //if

                _dataEntity = value;
                DataContext = value;

                if ( _dataEntity!=null ){
                    _dataEntity.Entity.PropertyChanged += EntityPropertyChanged;
                } //if
            }
        }

        /// <summary>
        /// The indexes property name.
        /// </summary>
        private const string IndexesPropertyName = "Indexes";

        /// <summary>
        /// Occurs when data entity property changed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void EntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == IndexesPropertyName){
                var selectedIndex = indexesDataGrid.SelectedIndex;
                indexesDataGrid.Rebind( DataGrid.ItemsSourceProperty );
                indexesDataGrid.SelectedIndex = selectedIndex;
            } //if
        }

        /// <summary>
        ///   Occurs when mouse left button down on the edit view.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void DataGridMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            //the stub of mouse handling.
            e.Handled = true;
        }

        /// <summary>
        ///   Returns a edit form
        /// </summary>
        /// <returns>The edit form instance.</returns>
        public FrameworkElement GetEditForm()
        {
            return EntityEdit;
        }

        /// <summary>
        ///   Occurs when user choised to add new attribute.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void AddEntityAttributeMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            _entityForm.RiseAddEntityAttribute();
        }

        /// <summary>
        ///   Occurs when click on delete attributed image.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void DeleteEntityAttributeMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            var element = (FrameworkElement) sender;
            var attribute = element.DataContext as EntityAttribute;

            if ( attribute != null ){
                _entityForm.RiseDeleteEntityAttribute( attribute );
            }
        }

        /// <summary>
        ///   Occurs when user press to OK button.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void OKButtonClick( object sender, RoutedEventArgs e )
        {
            _entityForm.StartShowViewForm();
        }

        /// <summary>
        ///   Occurs when edit form has been appeared.
        /// </summary>
        internal event EventHandler<AppearanceChangedEventArgs> AppearanceChanged;

        /// <summary>
        ///   Rises the AppearanceChanged event.
        /// </summary>
        /// <param name = "appeared">The appeared flag.</param>
        internal void RiseAppearanceChanged( bool appeared )
        {
            EventHandler<AppearanceChangedEventArgs> handler = AppearanceChanged;
            if ( handler != null ){
                handler( this, new AppearanceChangedEventArgs( appeared ) );
            }
        }

        /// <summary>
        /// Occurs when user clicks on add entity index image.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void AddEntityIndexLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            _entityForm.RiseAddEntityIndex();
        }

        /// <summary>
        ///  Occurs when user clicks on delete entity index image.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void DeleteEntityLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            var element = (FrameworkElement)sender;
            var attribute = element.DataContext as EntityIndex;

            if (attribute != null){
                _entityForm.RiseDeleteEntityIndex( attribute );
            }
        }
        
        /// <summary>
        /// User wish to ad new index attribute.
        /// </summary>
        /// <param name="entityIndex">The entity index.</param>
        /// <param name="proxy">The proxy.</param>
        private void AddIndexAttribute( EntityIndex entityIndex, IndexedAttributeProxy proxy )
        {
            if ( proxy != null && entityIndex != null ){
                _entityForm.RiseAddIndexAttribute( proxy.Attribute, entityIndex );
            } //if
        }

        /// <summary>
        /// User wish to remove attribute.
        /// </summary>
        /// <param name="entityIndex">The entity index.</param>
        /// <param name="proxy">The proxy.</param>
        private void RemoveIndexAttribute( EntityIndex entityIndex, IndexedAttributeProxy proxy )
        {
            if ( proxy != null && entityIndex != null && proxy.IndexAttribute != null ){
                _entityForm.RiseDeleteIndexAttribute( proxy.IndexAttribute, entityIndex );
            } //if
        }

        /// <summary>
        /// Occurs when user click on indexed check box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IndexedCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox) sender;
            var proxy = checkBox.DataContext as IndexedAttributeProxy;
            var entityIndex = indexesDataGrid.SelectedItem as EntityIndex;

            if ( proxy != null ){
                if ( proxy.Indexed ){
                    RemoveIndexAttribute( entityIndex, proxy );
                } //if
                else{
                    AddIndexAttribute( entityIndex, proxy );
                } //else
            } //if
        }

        /// <summary>
        /// Occurs when user selected a tab item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControlSelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            if ( ddlTabItem!=null && ddlTabItem.IsSelected ){
                UpdateDDLScript();
            } //if

            if ( asciiTabItem!=null && asciiTabItem.IsSelected ){
                UpdateAsciiView();
            } //if
        }

        /// <summary>
        /// Updates the entity ascii view.
        /// </summary>
        private void UpdateAsciiView()
        {
            asciiTextBox.Rebind( TextBox.TextProperty );
        }

        /// <summary>
        /// Updates the ddl script.
        /// </summary>
        private void UpdateDDLScript()
        {
            ddlTextBox.Rebind( TextBox.TextProperty );
        }

        /// <summary>
        /// Occurs when user want to reorder entity attributes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void AttributesReordering( object sender, DataGridReorderItemsEventArgs e )
        {
            _entityForm.RiseEntityAttributeReordering( (EntityAttribute) e.ReorderingItem, e.OldIndex, e.NewIndex );
        }

        /// <summary>
        /// Occurs when user want to copy ascii view of erd entity to clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyAsciiToClipboardClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText( asciiTextBox.Text );
        }

        /// <summary>
        /// Occurs when user want to copy the ddl script into clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyDDLToClipboardClick( object sender, RoutedEventArgs e )
        {
            Clipboard.SetText(ddlTextBox.Text);
        }

        /// <summary>
        /// Occurs when user whant to copy the notes text to system clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyNotesTextToClipboardClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(notesTextBox.Text);
        }
    }
}