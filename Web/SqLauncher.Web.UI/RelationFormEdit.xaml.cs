// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RelationFormEdit.xaml.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  02 07  22:33
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Controls;

using SqLauncher.Web.UI.Common;
using SqLauncher.Web.UI.Common.Shortcuts;

namespace SqLauncher.Web.UI
{
    public partial class RelationFormEdit : UserControl
    {
        public RelationFormEdit()
        {
            InitializeComponent();

            GotFocus += ( sender, args ) => ShortcutManager.Manager.StartResistentMode();
            LostFocus += ( sender, args ) => ShortcutManager.Manager.StopResistentMode();
        }

        /// <summary>
        ///   Occurs when user want to close relation form.
        /// </summary>
        public event EventHandler NeedToClose;

        /// <summary>
        ///   The event invocator for NeedToClose event.
        /// </summary>
        private void RiseNeedToClose()
        {
            EventHandler handler = NeedToClose;
            if ( handler != null ){
                handler( this, new EventArgs() );
            }
        }

        private void CloseButtonClick( object sender, System.Windows.RoutedEventArgs e )
        {
            RiseNeedToClose();
        }

        /// <summary>
        /// Occurs when edit form has been appeared.
        /// </summary>
        internal event EventHandler<AppearanceChangedEventArgs> AppearanceChanged;

        /// <summary>
        /// Rises the AppearanceChanged event.
        /// </summary>
        /// <param name="appeared">The appeared flag.</param>
        internal void RiseAppearanceChanged(bool appeared)
        {
            EventHandler<AppearanceChangedEventArgs> handler = AppearanceChanged;
            if (handler != null)
            {
                handler(this, new AppearanceChangedEventArgs(appeared));
            }
        }

        /// <summary>
        /// Occurs when user selected a tab item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ddlTabItem != null && ddlTabItem.IsSelected)
            {
                UpdateDDLScript();
            } //if
        }

        /// <summary>
        /// Updates the ddl script.
        /// </summary>
        private void UpdateDDLScript()
        {
            ddlTextBox.Rebind(TextBox.TextProperty);
        }

        /// <summary>
        /// Occurs when user want to copy the ddl script into clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyDDLToClipboardClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText( ddlTextBox.Text );
        }
    }
}