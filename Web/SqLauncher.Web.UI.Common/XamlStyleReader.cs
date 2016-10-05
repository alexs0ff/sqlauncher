// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   XamlStyleReader.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 08 7:25 PM
//   * Modified at: 2011  10 08  7:25 PM
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Media;

namespace SqLauncher.Web.UI.Common
{
    /// <summary>
    ///   The reader styles data in xaml documents.
    /// </summary>
    public class XamlStyleReader
    {
        /// <summary>
        ///   The default style uri.
        /// </summary>
        private const string DefaultStyleUri = "/SqLauncher.Web.UI;component/Styles.xaml";

        /// <summary>
        ///   The loaded dictionary.
        /// </summary>
        private readonly ResourceDictionary _handledDictionary;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Common.XamlStyleReader" /> class.
        /// </summary>
        public XamlStyleReader( string pathToXamlFile )
        {
            _handledDictionary  = new ResourceDictionary();
            _handledDictionary.Source = new Uri( pathToXamlFile, UriKind.Relative );
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Common.XamlStyleReader" /> class.
        /// </summary>
        public XamlStyleReader()
        {
            try{
                _handledDictionary = new ResourceDictionary();
                _handledDictionary.Source = new Uri(DefaultStyleUri, UriKind.Relative);
            } catch ( TypeInitializationException ){
                
            } //try
        }

        /// <summary>
        ///   Gets a style by it`s key.
        /// </summary>
        /// <param name = "key">The key of style.</param>
        /// <returns>The stored style.</returns>
        public Style GetStyle( string key )
        {
            if (_handledDictionary == null){
                return null;
            } //if
            return (Style) _handledDictionary[key];
        }

        /// <summary>
        ///   Gets the brush by it`s key.
        /// </summary>
        /// <param name = "key">The key of brush.</param>
        /// <returns>The stored brush.</returns>
        public Brush GetBrush( string key )
        {
            if ( _handledDictionary==null ){
                return new SolidColorBrush( Colors.Transparent );
            } //if

            return (Brush)_handledDictionary[key];
        }
    }
}