// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqlGenerationForm.xaml.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 25 10:47
//   * Modified at: 2011  11 25  13:55
// / ******************************************************************************/ 

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    public partial class SqlGenerationForm : ChildWindow, ISqlGenerationForm
    {
        /// <summary>
        ///   The pattern for file save dialog.
        /// </summary>
        private const string FilterPattern = "Sql Files (*.sql)|*.sql|All Files (*.*)|*.*";

        public SqlGenerationForm()
        {
            InitializeComponent();
            Closed += SqlGenerationFormClosed;
        }

        /// <summary>
        ///   Occurs when form closed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void SqlGenerationFormClosed( object sender, EventArgs e )
        {
            var dialogResult = ( (ChildWindow) sender ).DialogResult;
            if ( dialogResult != null && dialogResult.Value ){
                RiseDialogClosed( Model.DialogResult.OK );
            }
            else{
                RiseDialogClosed( Model.DialogResult.Cancel );
            }
        }

        /// <summary>
        ///   Occurs when user press on file choice button.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void ChoiceFileButtonClick( object sender, RoutedEventArgs e )
        {
            ChoiceFile();
        }

        /// <summary>
        ///   Starts choice a file to save.
        /// </summary>
        private void ChoiceFile()
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = FilterPattern;
            var showDialog = saveFileDialog.ShowDialog();
            if ( showDialog != null && showDialog.Value ){
                using ( var stream = saveFileDialog.OpenFile() ){
                    RiseSqlGenerating( stream,saveFileDialog.SafeFileName );
                }
            }
        }

        /// <summary>
        ///   Shows the form.
        /// </summary>
        public void Start()
        {
            Show();
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        public void CloseForm()
        {
            Close();
        }

        /// <summary>
        ///   Gets a collection for generated members.
        /// </summary>
        public ISqlGenerationFormViewState DataEntity
        {
            get { return DataContext as ISqlGenerationFormViewState; }
            set { DataContext = value; }
        }

        /// <summary>
        ///   Occurs when user want to generate sql and save it.
        /// </summary>
        public event EventHandler<DialogClosedEventArgs> DialogClosed;

        /// <summary>
        ///   The invocator for RiseDialog event.
        /// </summary>
        /// <param name = "result"></param>
        public void RiseDialogClosed( DialogResult result )
        {
            EventHandler<DialogClosedEventArgs> handler = DialogClosed;
            if ( handler != null ){
                handler( this, new DialogClosedEventArgs( result ) );
            }
        }

        /// <summary>
        ///   Occurs when user want to generate sql and save it.
        /// </summary>
        public event EventHandler<SqlGeneratingEventArgs> SqlGenerating;

        /// <summary>
        ///   The invocator for SqlGenerating event args.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <param name="filePath">The file path.</param>
        public void RiseSqlGenerating( Stream stream, string filePath )
        {
            EventHandler<SqlGeneratingEventArgs> handler = SqlGenerating;
            if ( handler != null ){
                handler( this, new SqlGeneratingEventArgs( stream, filePath ) );
            }
        }

        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}