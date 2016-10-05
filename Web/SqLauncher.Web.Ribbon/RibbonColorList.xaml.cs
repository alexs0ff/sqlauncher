// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RibbonColorList.xaml.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 09 25 10:46 AM
//   * Modified at: 2011  09 25  10:54 AM
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

using SqLauncher.Web.Ribbon.SilverlightColorPicker;

namespace SqLauncher.Web.Ribbon
{
    public partial class RibbonColorList : StackPanel
    {
        #region ctor

        public RibbonColorList()
        {
            InitializeComponent();
            MouseLeave += RibbonListMouseLeave;
            Loaded += RibbonListLoaded;
            AutoHide = false;
            startButton.DataContext = new SolidColorBrush( Colors.White );
            endButton.DataContext = new SolidColorBrush( Colors.White );
        }

        #endregion

        #region Methods

        private void Hide()
        {
            if ( _popup != null ){
                _popup.IsOpen = false;
            }
        }
      
        #endregion

        #region Events

        private void RibbonListLoaded( object sender, RoutedEventArgs e )
        {
            colorPicker.SelectedColorChanged += ColorPickerColorSelected;
            startButton.Click += StartButtonClick;
            endButton.Click += EndButtonClick;
        }

        private void RibbonListMouseLeave( object sender, MouseEventArgs e )
        {
            if ( AutoHide ){
                Hide();
            }
        }

        #endregion

        #region Fields

        private Popup _popup;

        public Popup Popup
        {
            get { return _popup; }
            internal set { _popup = value; }
        }

        private bool _autoHide = true;

        public bool AutoHide
        {
            get { return _autoHide; }
            set { _autoHide = value; }
        }

        private Color _color;

        public Color Color
        {
            get { return _color; }
            set
            {
                //if (value != _color) {

                if ( OnColorChanged != null ){
                    OnColorChanged( value );
                }

                _color = value;
            }
        }

        public delegate void ColorChangedHandler( Color c );

        public event ColorChangedHandler OnColorChanged;

        #endregion

        #region Brush 

        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register( "Brush", typeof ( Brush ), typeof ( RibbonColorList ),
                                                        new PropertyMetadata( new SolidColorBrush(Colors.White),OnBrushChange ) );

        private static void OnBrushChange( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var ribbonColorList = (RibbonColorList) d;
            ribbonColorList.preview.Fill = (Brush) e.NewValue;
            ribbonColorList.RiseBrushChanged();
        }

        /// <summary>
        /// The selected brush.
        /// </summary>
        public Brush Brush
        {
            get { return (Brush) GetValue( BrushProperty ); }
            set { SetValue( BrushProperty, value ); }
        }
     
        /// <summary>
        /// Occurs when brush changed.
        /// </summary>
        public event EventHandler BrushChanged;

        /// <summary>
        /// The invocator for brush changed event.
        /// </summary>
        private void RiseBrushChanged()
        {
            EventHandler handler = BrushChanged;
            if ( handler != null ){
                handler( this, new EventArgs() );
            }
        }

        /// <summary>
        /// Occurs when user choised a some brush.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void BrushButtonClick( object sender, RoutedEventArgs e )
        {
            var button = (Button) sender;
            Brush = (Brush) button.DataContext;
        }

        #endregion Brush

        #region Color choise 

        /// <summary>
        /// Occurs when user selected a color on color pallete.
        /// </summary>
        /// <param name="colorPickerEventArgs"></param>
        private void ColorPickerColorSelected( object sender, ColorPickerEventArgs colorPickerEventArgs )
        {
            Color = colorPickerEventArgs.SelectedColor;
            if (isGradient.IsChecked.HasValue && isGradient.IsChecked.Value){

                if ( _isStartColorChoise ){
                    startButton.DataContext = new SolidColorBrush(Color);
                    _color1 = Color;
                } //if
                else{
                    endButton.DataContext = new SolidColorBrush(Color);
                    _color2 = Color;
                } //else
                var gradient = new LinearGradientBrush();

                gradient.StartPoint = new Point( 0, 0 );
                gradient.EndPoint = new Point( 1, 1 );
                gradient.GradientStops = new GradientStopCollection{
                                                                       new GradientStop{Color = _color1,Offset = 0.0},
                                                                       new GradientStop{Color = _color2,Offset = 1.0}
                                                                   };
                preview.Fill = gradient;
            }
            else{
                preview.Fill = new SolidColorBrush( Color );
            } //else

            Brush = preview.Fill;

            //Hide();
        }

        /// <summary>
        /// The one color.
        /// </summary>
        private Color _color1;

        /// <summary>
        /// The two color.
        /// </summary>
        private Color _color2;

        private bool _isStartColorChoise = true;

        /// <summary>
        /// Occurs when user clicks on end buton.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args.</param>
        void EndButtonClick(object sender, RoutedEventArgs e)
        {
            _isStartColorChoise = false;
        }

        /// <summary>
        /// Occurs when user clicks on start buton.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args.</param>
        void StartButtonClick(object sender, RoutedEventArgs e)
        {
            _isStartColorChoise = true;
        }

        #endregion Color choise

    }
}