// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ZoomSlider.xaml.cs
//   * Project: SqLauncher.Web.Designer
//   * Description:
//   * Created at 2011 09 27 9:04 PM
//   * Modified at: 2011  09 27  9:57 PM
// / ******************************************************************************/ 

using System.Windows;
using System.Windows.Controls;

namespace SqLauncher.Web.Designer.Controls
{
    /// <summary>
    ///   Represents the Zoom slider control.
    /// </summary>
    public partial class ZoomSlider : UserControl
    {
        public ZoomSlider()
        {
            InitializeComponent();
            zoomSlider.ValueChanged += ( sender, args ) => Zoom = zoomSlider.Value;
        }

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register( "Zoom", typeof ( double ), typeof ( ZoomSlider ),
                                         new PropertyMetadata(default(double), ZoomChanged));

        private static void ZoomChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var zoom = d as ZoomSlider;
            zoom.zoomSlider.Value = (double) e.NewValue;
        }

        /// <summary>
        ///   The zoom property.
        /// </summary>
        public double Zoom
        {
            get { return (double) GetValue( ZoomProperty ); }
            set
            {
                SetValue( ZoomProperty, value );
            }
        }
    }
}