// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   MeasureProxy.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 08 5:15 PM
//   * Modified at: 2011  10 08  10:23 PM
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SqLauncher.Web.UI.Common.Measure
{
    /// <summary>
    ///   Reprsenets a proxy for diffirent measure units.
    /// </summary>
    public class MeasureProxy : DependencyObject
    {
        /// <summary>
        ///   Initializes type instance of the <see cref = "T:SqLauncher.Web.UI.Common.Measure.MeasureProxy" /> class.
        /// </summary>
        static MeasureProxy()
        {
            _units.Add( new PixelUnit() );
            _units.Add( new CentimeterUnit() );
            _units.Add( new InchUnit() );
            _units.Add( new MillimeterUnit() );
        }

        /// <summary>
        ///   The avalible units of measuring.
        /// </summary>
        private static readonly ICollection<IMeasureStrategy> _units = new ObservableCollection<IMeasureStrategy>();

        /// <summary>
        ///   The avalible units of measuring.
        /// </summary>
        public static ICollection<string> MeasureStrategies
        {
            get { return ( from strategy in _units select strategy.UnitName ).ToList(); }
        }

        /// <summary>
        ///   The current measure.
        /// </summary>
        private IMeasureStrategy _currentStrategy = new PixelUnit();

        #region Proxy

        public static readonly DependencyProperty MeasureProperty =
            DependencyProperty.Register( "Measure", typeof ( string ), typeof ( MeasureProxy ),
                                         new PropertyMetadata( OnMeasureNameChanged ) );

        /// <summary>
        ///   Occurs when measure property changed.
        /// </summary>
        /// <param name = "d">The dependency object.</param>
        /// <param name = "e">The DependencyPropertyChangedEventArgs.</param>
        private static void OnMeasureNameChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var proxy = (MeasureProxy) d;

            var choisedStrategy = _units.FirstOrDefault( s => s.UnitName == (string) e.NewValue );

            if ( choisedStrategy != null ){
                proxy._currentStrategy = choisedStrategy;
                proxy.UserMeasure = choisedStrategy.Convert( proxy.Value );
            } //if
        }

        /// <summary>
        ///   The current measure.
        /// </summary>
        public string Measure
        {
            get { return (string) GetValue( MeasureProperty ); }
            set { SetValue( MeasureProperty, value ); }
        }

        public static readonly DependencyProperty UserMeasureProperty =
            DependencyProperty.Register( "UserMeasure", typeof ( double ), typeof ( MeasureProxy ),
                                         new PropertyMetadata( OnUserMeasureChanged ) );

        /// <summary>
        ///   Occurs when UserMeasure property changed.
        /// </summary>
        /// <param name = "d">The dependency object.</param>
        /// <param name = "e">The DependencyPropertyChangedEventArgs.</param>
        private static void OnUserMeasureChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var proxy = (MeasureProxy) d;
            proxy.Value = proxy._currentStrategy.ConvertBack( (double) e.NewValue );
        }

        /// <summary>
        ///   The user visible measure.
        /// </summary>
        public double UserMeasure
        {
            get { return (double) GetValue( UserMeasureProperty ); }
            set { SetValue( UserMeasureProperty, value ); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register( "Value", typeof ( double ), typeof ( MeasureProxy ),
                                         new PropertyMetadata( OnValueChanged ) );

        /// <summary>
        ///   Occurs when Value property changed.
        /// </summary>
        /// <param name = "d">The dependency object.</param>
        /// <param name = "e">The DependencyPropertyChangedEventArgs.</param>
        private static void OnValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var proxy = (MeasureProxy) d;
            proxy.UserMeasure = proxy._currentStrategy.Convert( (double) e.NewValue );
        }

        /// <summary>
        ///   The adapted value.
        /// </summary>
        public double Value
        {
            get { return (double) GetValue( ValueProperty ); }
            set { SetValue( ValueProperty, value ); }
        }

        #endregion Proxy
    }
}