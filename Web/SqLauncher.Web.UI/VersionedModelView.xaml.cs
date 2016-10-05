// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   VersionedModelView.xaml.cs
//   * Project: SqLauncher.Web.UI
//   * Description: I need to make view like http://help.syncfusion.com/ug_84/User%20Interface/WPF/Grid/default.htm?turl=Documents%2Fexcellikeui.htm
//   * Created at 2011 11 03 19:50
//   * Modified at: 2011  11 07  13:01
// / ******************************************************************************/ 

using System;
using System.Linq;
using System.Windows;

using SqLauncher.Web.UI.Common.Shortcuts;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    /// Represents the view for model versions.
    /// </summary>
    public partial class VersionedModelView : IVersionedModelView
    {
        public VersionedModelView()
        {
            InitializeComponent();
            TabControl.AddNewItem += ( o, args ) => RiseVersionViewAdding();
            TabControl.SelectionChanged += ( o, args ) => RiseEditedModelViewChanged();
        }

        /// <summary>
        ///   Occurs when need to add new version.
        /// </summary>
        public event EventHandler<VersionViewAddingEventArgs> VersionViewAdding;

        /// <summary>
        /// The invocator of VersionViewAdding event.
        /// </summary>
        private void RiseVersionViewAdding()
        {
            EventHandler<VersionViewAddingEventArgs> handler = VersionViewAdding;

            if ( handler != null ){
                handler( this, new VersionViewAddingEventArgs() );
            }
        }

        /// <summary>
        /// Occurs when removing version.
        /// </summary>
        public event EventHandler<VersionViewRemovingEventArgs> VersionViewRemoving;

        /// <summary>
        /// The invocator of VersionViewRemoving event.
        /// </summary>
        /// <param name="version">The database version.</param>
        public void RiseVersionViewRemoving( DatabaseVersion version )
        {
            EventHandler<VersionViewRemovingEventArgs> handler = VersionViewRemoving;
            if ( handler != null ){
                handler( this, new VersionViewRemovingEventArgs( version ) );
            }
        }

        /// <summary>
        /// Adds new database version to view.
        /// </summary>
        /// <param name="version">The database version.</param>
        /// <returns>The removed view or null.</returns>
        public IModelView AddDatabaseVersion( DatabaseVersion version )
        {
            if ( DataEntity == null ){
                return null;
            } //if

            //check for added versions.
            if (!DataEntity.Versions.Contains(version))
            {
                DataEntity.Versions.Add(version);
            } //if

            version.Number = DataEntity.Versions.Count;

            //Select the last added tab
            TabControl.SelectedItem = version;

            return (IModelView) TabControl.GetContentByDataContext( version );
        }

        /// <summary>
        /// Removes the version from the view.
        /// </summary>
        /// <param name="version">The version to remove.</param>
        /// <returns>The removed view.</returns>
        public IModelView RemoveDatabaseVersion( DatabaseVersion version )
        {
            if ( version == null ){
                throw new ArgumentNullException( "version" );
            } //if

            var modelView = TabControl.GetContentByDataContext(version) as IModelView;

            if ( modelView!=null ){
                DataEntity.Versions.Remove( version );
            } //if

            return modelView;
        }

        /// <summary>
        /// Occurs when edited model view changed.
        /// </summary>
        public event EventHandler EditedModelViewChanged;

        /// <summary>
        /// The invocator for the EditedModelViewChanged event.
        /// </summary>
        private void RiseEditedModelViewChanged()
        {
            EventHandler handler = EditedModelViewChanged;
            if ( handler != null ){
                handler( this, new EventArgs() );
            }
        }

        /// <summary>
        /// Occurs when user clicks on close button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            var dataContext = ( (FrameworkElement) sender ).DataContext as DatabaseVersion;
            
            if ( dataContext!=null ){
                RiseVersionViewRemoving( dataContext );
            } //if
        }


        /// <summary>
        /// Gets the current model view.
        /// <remarks>If no tabs then reterns null.</remarks>
        /// </summary>
        public IModelView EditedModelView
        {
            get
            {
                return TabControl.SelectedContent as IModelView;
            }
        }

        /// <summary>
        /// The data entity.
        /// </summary>
        public DatabaseDocument DataEntity
        {
            get { return DataContext as DatabaseDocument; }
            set { DataContext = value; }
        }

        #region Version view loaded 

        /// <summary>
        /// Occurs when a model view has been loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ModelViewLoaded( object sender, RoutedEventArgs e )
        {
            var modelViewState = ( (FrameworkElement) sender ).DataContext as IModelViewState;

            if ( modelViewState!=null ){

                var databaseVersion =
                    DataEntity.Versions.FirstOrDefault(
                        version => ReferenceEquals( version.ModelViewState, modelViewState ) );

                if (databaseVersion != null){
                    RiseVersionViewLoaded( databaseVersion );
                } //if    
            } //if
            
        }

        /// <summary>
        /// Occurs when version view loaded.
        /// </summary>
        public event EventHandler<VersionViewLoadedEventArgs> VersionViewLoaded;

        /// <summary>
        /// The event invocator for VersionViewLoaded event.
        /// </summary>
        /// <param name="version">The database version of loaded view.</param>
        private void RiseVersionViewLoaded( DatabaseVersion version )
        {
            EventHandler<VersionViewLoadedEventArgs> handler = VersionViewLoaded;
            if ( handler != null ){
                handler( this, new VersionViewLoadedEventArgs( version ) );
            }
        }

        #endregion Version view loaded

        private void VersionInfoGotFocus(object sender, RoutedEventArgs e)
        {
            ShortcutManager.Manager.StartResistentMode();
        }

        private void VersionInfoLostFocus(object sender, RoutedEventArgs e)
        {
            ShortcutManager.Manager.StopResistentMode();
        }
    }
}