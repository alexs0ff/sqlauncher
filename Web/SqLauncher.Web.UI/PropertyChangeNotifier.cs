// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   PropertyChangeNotifier.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 31 10:12 PM
//   * Modified at: 2011  09 01  8:58 PM
// / ******************************************************************************/ 

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   represents the listener for a property.
    ///   http://agsmith.wordpress.com/2008/04/07/propertydescriptor-addvaluechanged-alternative/
    /// </summary>
    public sealed class PropertyChangeNotifier :
        DependencyObject,
        IDisposable
    {
        #region Member Variables

        private WeakReference _propertySource;

        #endregion // Member Variables

        #region Constructor

        public PropertyChangeNotifier( DependencyObject propertySource, string path )
            : this( propertySource, new PropertyPath( path ) )
        {
        }

        public PropertyChangeNotifier( DependencyObject propertySource, DependencyProperty property )
            : this( propertySource, new PropertyPath( property ) )
        {
        }

        public PropertyChangeNotifier( DependencyObject propertySource, PropertyPath property )
        {
            if ( null == propertySource ){
                throw new ArgumentNullException( "propertySource" );
            }
            if ( null == property ){
                throw new ArgumentNullException( "property" );
            }

            _propertySource = new WeakReference( propertySource );
            Binding binding = new Binding();
            binding.Path = property;
            binding.Mode = BindingMode.OneWay;
            binding.Source = propertySource;
            BindingOperations.SetBinding( this, ValueProperty, binding );
        }

        #endregion // Constructor

        #region PropertySource

        public DependencyObject PropertySource
        {
            get
            {
                try{
                    // note, it is possible that accessing the target property
                    // will result in an exception so i’ve wrapped this check
                    // in a try catch
                    return _propertySource.IsAlive
                               ? _propertySource.Target as DependencyObject
                               : null;
                } catch{
                    return null;
                }
            }
        }

        #endregion // PropertySource

        #region Value

        /// <summary>
        ///   Identifies the dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register( "Value",
                                         typeof ( object ),
                                         typeof ( PropertyChangeNotifier ),
                                         new PropertyMetadata( null, OnPropertyChanged ) );

        private static void OnPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            PropertyChangeNotifier notifier = (PropertyChangeNotifier) d;
            if ( null != notifier.ValueChanged ){
                notifier.ValueChanged( notifier, EventArgs.Empty );
            }
        }

        /// <summary>
        ///   Returns/sets the value of the property
        /// </summary>
        /// <seealso cref =   ”ValueProperty” />
        [Description( "Returns/sets the value of the property" )]
        [Category( "Behavior" )]
        [Bindable( true )]
        public object Value
        {
            get { return this.GetValue( ValueProperty ); }
            set { this.SetValue( ValueProperty, value ); }
        }

        #endregion //Value

        #region Events

        public event EventHandler ValueChanged;

        #endregion // Events

        /// <summary>
        /// Some associated datasource.
        /// </summary>
        public object Tag { get; set; }

        #region IDisposable Members

        public void Dispose()
        {

            ClearValue( ValueProperty );
        }

        #endregion
    }
}