using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SqLauncher.Web.Model;
using SqLauncher.Web.Ribbon;
using SqLauncher.Web.UI.Common.Shortcuts;

namespace SqLauncher.Web.Designer
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            ApplicationController.Controller.InitializeUI( this, mainTabControl );
            
            ShortcutManager.Manager.RegisterRoot( LayoutRoot );
            
            ShortcutManager.Manager.Register( new ShortcutDescriptor( Key.C, ModifierKeys.Control ),
                                              Copy, true);
            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.V, ModifierKeys.Control),
                                              Paste, true);

            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.Insert, ModifierKeys.Control),
                                              Copy, true);
            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.Insert, ModifierKeys.Shift),
                                              Paste, true);

            ShortcutManager.Manager.Register( new ShortcutDescriptor( Key.D, ModifierKeys.Control ),
                                              Duplicate, true);

            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.X, ModifierKeys.Control),
                                              Cut, true);

            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.Z, ModifierKeys.Control),
                                              Undo,true);
            ShortcutManager.Manager.Register( new ShortcutDescriptor( Key.Y, ModifierKeys.Control ),
                                              Redo, true );

            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.G, ModifierKeys.Control),
                                            GeneratingSql);

            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.N, ModifierKeys.Control),
                                              CreateNewDatabaseSchema);

            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.Delete),
                                              RemoveSelectedItems, true);

            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.E),
                                              CreateAndPlaceEntityForm, true);
            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.R),
                                              CreateAndPlaceNonIdentifyingRelationForm, true);
            ShortcutManager.Manager.Register( new ShortcutDescriptor( Key.Escape ),
                                              Cancel, true );

            ShortcutManager.Manager.Register(new ShortcutDescriptor(Key.J, ModifierKeys.Control),
                                              AddNewVersion, true);
        }

        #region Start button handlers 
        
        private static void SaveSchema()
        {
            ApplicationController.Controller.SaveCurrentModelView();
        }

        private void OpenSchema()
        {
            ApplicationController.Controller.LoadModelViewFromFile();
        }

        private static void CloseSchema()
        {
            ApplicationController.Controller.CloseCurrentModelView();
        }

        private static void GeneratingSql()
        {
            ApplicationController.Controller.GetModelControllerHandler().StartGeneratingSql();
        }

        private static void Undo()
        {
            ApplicationController.Controller.GetModelControllerHandler().Undo();
        }

        private static void Redo()
        {
            ApplicationController.Controller.GetModelControllerHandler().Redo();
        }

        private static void CreateAndPlaceEntityForm()
        {
            ApplicationController.Controller.GetModelControllerHandler().CreateAndPlaceEntityForm();
        }

        private static void CreateAndPlaceNonIdentifyingRelationForm()
        {
            ApplicationController.Controller.GetModelControllerHandler().CreateAndPlaceRelationForm(
                RelationshipType.NonIdentifying );
        }

        private void CreateAndPlaceIdentifyingRelationForm()
        {
            ApplicationController.Controller.GetModelControllerHandler().CreateAndPlaceRelationForm(
                RelationshipType.Identifying);
        }

        private void CreateAndPlaceInformativeRelationForm()
        {
            ApplicationController.Controller.GetModelControllerHandler().CreateAndPlaceRelationForm(
               RelationshipType.Informative);
        }

        private static void Duplicate()
        {
            ApplicationController.Controller.GetModelControllerHandler().Duplicate();
        }

        private static void Copy()
        {
            ApplicationController.Controller.GetModelControllerHandler().Copy();
        }

        private static void Paste()
        {
            ApplicationController.Controller.GetModelControllerHandler().Paste();
        }

        private static void Cut()
        {
            ApplicationController.Controller.GetModelControllerHandler().Cut();
        }

        private static void RemoveSelectedItems()
        {
            ApplicationController.Controller.GetModelControllerHandler().RemoveSelectedItems();
        }

        private static void CreateNewDatabaseSchema()
        {
            ApplicationController.Controller.CreateSqLiteModel();
        }

        private static void Cancel()
        {
            ApplicationController.Controller.GetModelControllerHandler().CancelCurrentOperation();
        }

        private static void AddNewVersion()
        {
            ApplicationController.Controller.AddNewVersionIntoCurrentModelView();
        }

        #endregion Start button handlers

        private void UndoRibbonButtonMouseClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Undo();
        }

        private void RedoRibbonButtonMouseClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Redo();
        }

        private void CreateSqLiteDataBaseMouseLeftButtonUp(object sender, RoutedEventArgs routedEventArgs)
        {
            CreateNewDatabaseSchema();
        }

        private void InsertNewEntity(object sender, RoutedEventArgs e)
        {
            CreateAndPlaceEntityForm();
        }

        private void InsertNewNonIdentifyingRelation(object sender, EventArgs eventArgs)
        {
            CreateAndPlaceNonIdentifyingRelationForm();
        }

        private void InsertNewIdentifyingRelation(object sender, EventArgs e)
        {
            CreateAndPlaceIdentifyingRelationForm();
        }

        private void InsertNewInformativeRelation(object sender, EventArgs e)
        {
            CreateAndPlaceInformativeRelationForm();
        }

        private void StartSave( object sender, RoutedEventArgs e )
        {
            GeneratingSql();
        }

        private void CopyButtonOnClick(object sender, RoutedEventArgs e)
        {
            Copy();
        }

        private void PasteButtonOnClick(object sender, RoutedEventArgs e)
        {
            Paste();
        }

        private void DuplicateButtonOnClick(object sender, RoutedEventArgs e)
        {
            Duplicate();
        }

        private void OpenFileMouseClick(object sender, RoutedEventArgs e)
        {
            OpenSchema();
        }

        private void SaveFileMouseClick(object sender, RoutedEventArgs e)
        {
            SaveSchema();
        }

        private void RemoveButtonMouseClick( object sender, RoutedEventArgs e )
        {
            RemoveSelectedItems();
        }

        private void StartButtonItemCreateNewSchemaClick(object sender, EventArgs e)
        {
            CreateNewDatabaseSchema();
        }

        private void StartButtonItemOpenExistingSchemaClick(object sender, EventArgs e)
        {
            OpenSchema();
        }

        private void StartButtonItemSaveSchemaClick(object sender, EventArgs e)
        {
            SaveSchema();
        }

        private void StartButtonItemGenerateSqlClick(object sender, EventArgs e)
        {
            GeneratingSql();
        }

        private void StartButtonItemCloseClick(object sender, EventArgs e)
        {
            CloseSchema();
        }

        private void CutButtonOnClick(object sender, RoutedEventArgs e)
        {
            Cut();
        }

        private void BackgroundBrushChanged( object sender, BrushChangedEventArgs e )
        {
            ApplicationController.Controller.GetModelControllerHandler().RegisterEntityFormsBackgroundBrushChange();
        }

        #region Ribbon workarounds 
        
        private void PhysicalViewTextBlockMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            ApplicationController.Controller.GetModelControllerHandler().GetIteractionProvider().IteractionState.
                PhysicalView =
                !ApplicationController.Controller.GetModelControllerHandler().GetIteractionProvider().IteractionState.
                    PhysicalView;
        }

        #endregion Ribbon workarounds

        private void OnMailButtonClick(object sender, RoutedEventArgs e)
        {
            var contactForm = new ContactForm();
            contactForm.Show();
        }
    }
}
