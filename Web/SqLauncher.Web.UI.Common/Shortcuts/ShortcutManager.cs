// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ShortcutManager.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2012 02 06 20:40
//   * Modified at: 2012  02 06  21:51
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Input;

namespace SqLauncher.Web.UI.Common.Shortcuts
{
    /// <summary>
    ///   The manager for shortcut combinations.
    /// </summary>
    public class ShortcutManager
    {
        #region Singleton

        /// <summary>
        ///   The singleton instance.
        /// </summary>
        private static volatile ShortcutManager _instance;

        private static readonly object _staticSyncRoot = new object();

        private readonly object _syncRoot = new object();

        /// <summary>
        /// The key code lookup.
        /// </summary>
        internal static readonly Dictionary<int, Key> KeyCodeLookUp = new Dictionary<int, Key>();

        /// <summary>
        /// The manager.
        /// </summary>
        public static ShortcutManager Manager
        {
            get
            {
                //The double checked pattern
                if ( _instance == null ){
                    lock ( _staticSyncRoot ){
                        if ( _instance == null ){
                            _instance = new ShortcutManager();
                        } //if
                    } //lock
                } //if
                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.UI.Common.Shortcuts.ShortcutManager"/> class.
        /// </summary>
        private ShortcutManager()
        {
            //http://www.rajneeshnoonia.com/blog/2010/05/silverlight-hot-keys/
            //Digits
            for (var i = (int)Key.D0; i <= (int)Key.D9; i++){
                KeyCodeLookUp.Add(  i + 28,(Key) i);
            }
            //Chars
            for (var i = (int)Key.A; i <= (int)Key.Z; i++){
                KeyCodeLookUp.Add(i + 35, (Key)i);
            }

            //functional Key
            for (var i = (int)Key.F1; i <= (int)Key.F12; i++){
                KeyCodeLookUp.Add( i + 65, (Key) i );
            }

            KeyCodeLookUp.Add( 1, Key.Escape );
        }

        #endregion Singleton

        /// <summary>
        ///   The shortcuts dictionary.
        /// </summary>
        private readonly IDictionary<ShortcutDescriptor, Action> _shortcuts =
            new Dictionary<ShortcutDescriptor, Action>();

        /// <summary>
        ///   The registred shortcuts.
        /// </summary>
        public ReadOnlyCollection<ShortcutDescriptor> RegistredShortcuts
        {
            get { return new ReadOnlyCollection<ShortcutDescriptor>( _shortcuts.Keys.ToList() ); }
        }

        #region UI Resistence 

        /// <summary>
        /// The actions.
        /// </summary>
        private readonly ICollection<ShortcutDescriptor> _resistentShortcuts = new Collection<ShortcutDescriptor>();

        /// <summary>
        /// The flag of resistent mode.
        /// </summary>
        private bool _isResistentMode;

        /// <summary>
        /// Freezes all resistent actions.
        /// </summary>
        public void StartResistentMode()
        {
            _isResistentMode = true;
        }

        /// <summary>
        /// Returned in normal mode.
        /// </summary>
        public void StopResistentMode()
        {
            _isResistentMode = false;
        }

        #endregion UI Resistence 

        /// <summary>
        ///   Registres a new shortcuts with callback.
        /// </summary>
        /// <param name = "descriptor">The shortcut descriptor.</param>
        /// <param name = "action">The callback, what will be invoke.</param>
        public void Register( ShortcutDescriptor descriptor, Action action )
        {
            Register( descriptor, action, false );
        }

        /// <summary>
        ///   Registres a new shortcuts with callback.
        /// </summary>
        /// <param name = "descriptor">The shortcut descriptor.</param>
        /// <param name = "action">The callback, what will be invoke.</param>
        /// <param name="isResistentAction">The action resistence.</param>
        public void Register(ShortcutDescriptor descriptor, Action action, bool isResistentAction)
        {
            lock ( _syncRoot ){
                _shortcuts[descriptor] = action;

                if ( isResistentAction ){
                    _resistentShortcuts.Add( descriptor );
                } //if

            } //lock
        }

        /// <summary>
        ///   The main framework element.
        /// </summary>
        private FrameworkElement _frameworkElement;

        /// <summary>
        ///   Setups the ui root element for key event listening.
        /// </summary>
        /// <param name = "element">The main ui element.</param>
        public void RegisterRoot( FrameworkElement element )
        {
            _frameworkElement = element;
            _frameworkElement.KeyDown += RootKeyUp;

            try{
                HtmlDocument document = HtmlPage.Document;
                EventHandler<HtmlEventArgs> keyDownHandler = OnHtmlPageKeyUp;
                document.AttachEvent("onkeyup", keyDownHandler);
                HtmlPage.Document.Body.Invoke("focus", new object[] { });
            } catch{
            } //try
        }

        /// <summary>
        /// Occurs when user key up on html
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHtmlPageKeyUp( object sender, HtmlEventArgs e )
        {
            if (KeyCodeLookUp.ContainsKey( e.KeyCode ))
            {
                ProcessKeyUp(KeyCodeLookUp[e.KeyCode]);
            } //if
        }

        /// <summary>
        ///   Stops the listeneng of key shortcuts.
        /// </summary>
        public void StopListening()
        {
            _frameworkElement.KeyDown -= RootKeyUp;

            try{
                HtmlDocument document = HtmlPage.Document;
                EventHandler<HtmlEventArgs> keyDownHandler = OnHtmlPageKeyUp;
                document.DetachEvent("onkeyup", keyDownHandler);
            } catch{
            } //try
        }

        /// <summary>
        ///   Occurs when user pressed key on ui element.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void RootKeyUp( object sender, KeyEventArgs e )
        {
            ProcessKeyUp( e.Key );
        }

        private void ProcessKeyUp(Key key)
        {
            lock ( _syncRoot ){
                var shortcut = ShortcutDescriptor.Create( key );

                if ( _isResistentMode ){

                    if ( !_resistentShortcuts.Contains( shortcut ) && _shortcuts.ContainsKey( shortcut ) ){
                        _shortcuts[shortcut]();
                    } //if

                } //if
                else{

                    if ( _shortcuts.ContainsKey( shortcut ) ){
                        _shortcuts[shortcut]();
                    } //if

                } //else

            } //lock
        }
    }
}