// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ApplicationController.cs
//   * Project: SqLauncher.Web.Designer
//   * Description:
//   * Created at 2011 09 25 12:47 PM
//   * Modified at: 2011  10 01  12:51 PM
// / ******************************************************************************/ 

using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using SqLauncher.Web.Controller;
using SqLauncher.Web.Controller.DataModelInterceptions;
using SqLauncher.Web.Model;
using SqLauncher.Web.UI;
using SqLauncher.Web.UI.Common.Shortcuts;
using SqLauncher.Web.UI.Common.UserControls;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Designer
{
    /// <summary>
    ///   Represents a global application controller.
    /// </summary>
    public class ApplicationController
    {
        #region Singleton 

        /// <summary>
        ///   The controller.
        /// </summary>
        public static ApplicationController Controller
        {
            get { return _instance; }
        }

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly ApplicationController _instance = new ApplicationController();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.ModelController" /> class.
        /// </summary>
        private ApplicationController()
        {
        }

        /// <summary>
        ///   Synchronization object.
        /// </summary>
        private readonly object _syncRoot = new object();

        #endregion Singleton

        #region UI 

        /// <summary>
        ///   The document tab panel.
        /// </summary>
        private TabControl _viewTabPanel;

        /// <summary>
        ///   The main page.
        /// </summary>
        private MainPage _mainPage;

        /// <summary>
        ///   Initializes the main ui controls.
        /// </summary>
        /// <param name = "mainPage">The main page.</param>
        /// <param name = "viewPanel">The document view panel.</param>
        public void InitializeUI( MainPage mainPage, TabControl viewPanel )
        {
            _viewTabPanel = viewPanel;
            _mainPage = mainPage;
            _mainPage.DataContext = UserIteractionProvider.Default;
            _viewTabPanel.SelectionChanged += ViewTabPanelSelectionChanged;
        }

        /// <summary>
        ///   Occurs when tab item has been choised.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void ViewTabPanelSelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            UpdateIteractionProvider();
        }

        /// <summary>
        /// Updates the current iteraction provider.
        /// </summary>
        private void UpdateIteractionProvider()
        {
            _mainPage.DataContext = GetModelControllerHandler().GetIteractionProvider();
        }

        #endregion UI

        #region Controllers and views 

        /// <summary>
        ///   The current selected controller.
        /// </summary>
        public IViewController CurrentController
        {
            get
            {
                IViewController result = null;

                if (CurrentVersionedModelViewManager != null)
                {
                    result = CurrentVersionedModelViewManager.CurrentController;
                } //if

                return result;
            }
        }

        /// <summary>
        /// The current versioned model view manager.
        /// </summary>
        public VersionedModelViewManager  CurrentVersionedModelViewManager
        {
            get
            {
                TabItem tabItem = null;

                if (_viewTabPanel.SelectedItem != null)
                {
                    tabItem = (TabItem)_viewTabPanel.SelectedItem;
                } //if

                VersionedModelViewManager versionedModelViewManager = null;

                if (tabItem != null)
                {
                    versionedModelViewManager = (VersionedModelViewManager)tabItem.Tag;
                } //if

                return versionedModelViewManager;
            }
        }

        #endregion Controllers and views

        /// <summary>
        /// Closes the current model.
        /// </summary>
        public void CloseCurrentModelView()
        {
            if (CurrentVersionedModelViewManager==null)
            {
                return;
            } //if

            CurrentVersionedModelViewManager.CurrentModelViewChanged -= CurrentModelViewChanged;
            _viewTabPanel.Items.Remove( _viewTabPanel.SelectedItem );
        }

        /// <summary>
        /// Adds the new version into current model view.
        /// </summary>
        public void AddNewVersionIntoCurrentModelView()
        {
            if (CurrentVersionedModelViewManager == null)
            {
                return;
            } //if

            CurrentVersionedModelViewManager.AddNewModelView();
        }

        /// <summary>
        ///   Cretaes a new tab for the SqLite Model controller.
        /// </summary>
        public void CreateSqLiteModel()
        {
            var wiring = ContainerWiring.SetUp( new SqLiteModelInterception() );
            DatabaseDocument document = CreateEmptyDocument( wiring );
            CreateSqLiteModel( wiring, document );
        }

        /// <summary>
        /// Creates the new empty database document.
        /// </summary>
        /// <param name="wiring">The model wiring.</param>
        /// <returns>The created document.</returns>
        private DatabaseDocument CreateEmptyDocument( ContainerWiring wiring )
        {
            var result = wiring.CreateInstance<DatabaseDocument>();
            result.Name = "New SqLite";

            var version = wiring.CreateInstance<DatabaseVersion>();
            version.Number = 1;
            version.Name = "Version 1";
            version.CreateDate = DateTime.Now;
            result.Versions.Add( version );

            return result;
        }

        /// <summary>
        /// Creates a view by database document object.
        /// </summary>
        /// <param name="wiring">The model interception.</param>
        /// <param name="document">The database document.</param>
        public void CreateSqLiteModel(ContainerWiring wiring, DatabaseDocument document)
        {
            var versionedModelView = new VersionedModelView();
            versionedModelView.DataEntity = document;
            var versionedModelManager = new VersionedModelViewManager( versionedModelView, wiring );
            versionedModelManager.CurrentModelViewChanged += CurrentModelViewChanged;
            var newTab = new TabItem();
            newTab.DataContext = document;

            newTab.Tag = versionedModelManager;
            
            var tabName = new ClickToEditTextboxControl();

            var nameBinding = new Binding("Name");
            nameBinding.Mode = BindingMode.TwoWay;

            tabName.SetBinding( ClickToEditTextboxControl.TextProperty, nameBinding );
            
            tabName.GotFocus += (sender, args) => ShortcutManager.Manager.StartResistentMode();
            tabName.LostFocus += (sender, args) => ShortcutManager.Manager.StopResistentMode();

            newTab.Header = tabName;

            newTab.Content = versionedModelView;
            
            _viewTabPanel.Items.Add( newTab );
            _viewTabPanel.SelectedItem = newTab;
        }

        #region Serializing 

        private SerializerManager _currentSerializerManager;

        /// <summary>
        /// Saves the current model.
        /// </summary>
        public void SaveCurrentModelView()
        {
            if ( _currentSerializerManager != null ){
                _currentSerializerManager.Stop();
            } //if

            if ( CurrentVersionedModelViewManager == null ){
                return;
            } //if

            _currentSerializerManager = new SerializerManager( new SerializerDialog(),
                                                               CurrentVersionedModelViewManager.Wiring );
            _currentSerializerManager.SaveDatabaseDocument(
                CurrentVersionedModelViewManager.VersionedModelView.DataEntity );
        }

        /// <summary>
        /// Loads the model view.
        /// </summary>
        public void LoadModelViewFromFile()
        {
            if (_currentSerializerManager != null)
            {
                _currentSerializerManager.Stop();
            } //if
            
            //TODO remove explicit interception 
            
            _currentSerializerManager = new SerializerManager( new SerializerDialog(), ContainerWiring.SetUp( new SqLiteModelInterception() ) );
            _currentSerializerManager.LoadDatabaseDocument();
        }

        #endregion Serializing

        /// <summary>
        /// Occurs when edited version changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void CurrentModelViewChanged(object sender, System.EventArgs e)
        {
            UpdateIteractionProvider();
        }

        #region Delegating of Model controller 

        /// <summary>
        ///   Gets a model controller handler for current controller,
        /// </summary>
        /// <returns>The instance of model controller.</returns>
        public ModelControllerHandler GetModelControllerHandler()
        {
            return new ModelControllerHandler( CurrentController );
        }

        #endregion Delegating of Model controller
    }
}