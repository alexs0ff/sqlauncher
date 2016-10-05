// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ColorPicker.xaml.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 10 02 3:27 PM
//   * Modified at: 2011  10 02  3:28 PM
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ColorPicker.Classes;

namespace SqLauncher.Web.Ribbon.SilverlightColorPicker
{
    public partial class ColorPicker : UserControl
    {
        public event EventHandler<ColorPickerEventArgs> SelectedColorChanged;

        public Color SelectedColor
        {
            get { return _selectedColor; }
        }

        private Point _cursor;

        private bool _isDragable;

        private Point _clickedCursor;

        private Color _selectedColor;

        private List<Pixel> PixelsCollection = new List<Pixel>();

        public ColorPicker()
        {
            InitializeComponent();
            this.Loaded += CtrlColorPickerLoaded;
        }

        private void CtrlColorPickerLoaded( object sender, RoutedEventArgs e )
        {
            CreatePallete();

            Picker.MouseLeftButtonDown += PickerMouseLeftButtonDown;
            Picker.MouseLeftButtonUp += PickerMouseLeftButtonUp;
            Picker.MouseMove += PickerMouseMove;

            ColorBar.MouseMove += ColorBarMouseMove;
            ColorBar.MouseLeftButtonDown += ColorBarMouseLeftButtonDown;
            ColorBar.MouseLeftButtonUp += ColorBarMouseLeftButtonUp;

            GetSelectedColor();
        }

        private void ColorBarMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            _isDragable = false;
            //ColorPickerHandle.ReleaseMouseCapture();
        }

        private void ColorBarMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            _isDragable = true;
            //ColorPickerHandle.CaptureMouse();
            ColorPickerHandle.SetValue( Canvas.TopProperty, e.GetPosition( ColorBar ).Y );
            SetColorFromColorBar( e.GetPosition( ColorBar ) );
        }

        private void ColorBarMouseMove( object sender, MouseEventArgs e )
        {
            if ( _isDragable ){
                ColorPickerHandle.SetValue( Canvas.TopProperty, _cursor.Y );
                SetColorFromColorBar( e.GetPosition( ColorBar ) );
            }
        }

        private void SetColorFromColorBar( Point p )
        {
            _cursor = p;

            WriteableBitmap w = new WriteableBitmap( ColorBar, null );

            int colorAsInt = w.Pixels[Convert.ToInt32( _cursor.Y*w.PixelWidth + _cursor.X )];

            Color c = Color.FromArgb( (byte) ( ( colorAsInt >> 0x18 ) & 0xff ),
                                      (byte) ( ( colorAsInt >> 0x10 ) & 0xff ),
                                      (byte) ( ( colorAsInt >> 8 ) & 0xff ),
                                      (byte) ( colorAsInt & 0xff ) );

            primaryColor.Fill = new SolidColorBrush( c );

            GetSelectedColor();
        }

        private void PickerMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            _isDragable = false;
            Picker.ReleaseMouseCapture();
            if (SelectedColorChanged != null)
            {
                SelectedColorChanged(this, new ColorPickerEventArgs(SelectedColor));
            }
        }

        private void PickerMouseMove( object sender, MouseEventArgs e )
        {
            _cursor = e.GetPosition( SubPalette );

            if ( _isDragable ){
                if ( _cursor.X < SubPalette.ActualWidth && _cursor.X > 0 && _cursor.Y < SubPalette
                                                                                            .ActualHeight &&
                     _cursor.Y > 0 ){
                    double x = _cursor.X - _clickedCursor.X;
                    double y = _cursor.Y - _clickedCursor.Y;

                    if ( x > 0 ){
                        Picker.SetValue( Canvas.LeftProperty, x );
                    }

                    if ( y > 0 ){
                        Picker.SetValue( Canvas.TopProperty, y );
                    }

                    GetSelectedColor();
                }
            }
        }

        private void PickerMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            _isDragable = true;
            Picker.CaptureMouse();
            _clickedCursor = e.GetPosition( Picker );
        }

        private void BuildBitmap()
        {
            int imageWidth = 1;
            int imageHeight = 306;

            int count = 0;

            WriteableBitmap b = new WriteableBitmap( imageWidth, imageHeight );
            for ( int x = 0; x < imageWidth; x++ ){
                for ( int y = 0; y < imageHeight; y++ ){
                    // generate a color in 32bit format

                    Pixel p = PixelsCollection[count];

                    byte[] components = new byte[4];
                    components[0] = p.B; //(byte)(x % 255);        // blue
                    components[1] = p.G; //(byte)(y % 255);        // green
                    components[2] = p.R; //(byte)(x * y % 255);    // red
                    components[3] = p.A; //255;      // unused

                    int pixelValue = BitConverter.ToInt32( components, 0 );

                    // Set the value for the 
                    b.Pixels[x*imageHeight + y] = pixelValue;
                    count++;
                }
            }

            b.Invalidate();
            ColorBar.Source = b;
        }

        private void CreatePallete()
        {
            int Range = 51;
            int ColorIncrement = 5;

            int RedChannel = 255;
            int GreenChannel = 0;
            int BlueChannel = 0;

            //red maximum
            //Increase Green Channel
            for ( int v = 0; v < Range; v++ ){
                Pixel p = new Pixel();
                p.A = 255;
                p.R = (byte) RedChannel;
                p.G = (byte) GreenChannel;
                p.B = (byte) BlueChannel;

                GreenChannel = GreenChannel + ColorIncrement;

                PixelsCollection.Add( p );
            }
            //green maximum
            //Decrease Red Channel
            for ( int v = 0; v < Range; v++ ){
                Pixel p = new Pixel();
                p.A = 255;
                p.R = (byte) RedChannel;
                p.G = (byte) GreenChannel;
                p.B = (byte) BlueChannel;

                RedChannel = RedChannel - ColorIncrement;

                PixelsCollection.Add( p );
            }

            // green maximum
            //Increase Blue Channel
            for ( int v = 0; v < Range; v++ ){
                Pixel p = new Pixel();
                p.A = 255;
                p.R = (byte) RedChannel;
                p.G = (byte) GreenChannel;
                p.B = (byte) BlueChannel;

                BlueChannel = BlueChannel + ColorIncrement;

                PixelsCollection.Add( p );
            }

            // blue maximum
            //Decrease Green Channel
            for ( int v = 0; v < Range; v++ ){
                Pixel p = new Pixel();
                p.A = 255;
                p.R = (byte) RedChannel;
                p.G = (byte) GreenChannel;
                p.B = (byte) BlueChannel;

                GreenChannel = GreenChannel - ColorIncrement;

                PixelsCollection.Add( p );
            }

            // blue maximum
            //Increase Red Channel
            for ( int v = 0; v < Range; v++ ){
                Pixel p = new Pixel();
                p.A = 255;
                p.R = (byte) RedChannel;
                p.G = (byte) GreenChannel;
                p.B = (byte) BlueChannel;

                RedChannel = RedChannel + ColorIncrement;

                PixelsCollection.Add( p );
            }

            // red maximum
            //Decrease Blue Channel
            for ( int v = 0; v < Range; v++ ){
                Pixel p = new Pixel();
                p.A = 255;
                p.R = (byte) RedChannel;
                p.G = (byte) GreenChannel;
                p.B = (byte) BlueChannel;

                BlueChannel = BlueChannel - ColorIncrement;

                PixelsCollection.Add( p );
            }

            BuildBitmap();
        }

        private void GetSelectedColor()
        {
            GeneralTransform gt = Picker.TransformToVisual( SubPalette );
            Point offset = gt.Transform( new Point( 0, 0 ) );

            WriteableBitmap w = new WriteableBitmap( SubPalette, null );

            int colorAsInt = w.Pixels[Convert.ToInt32( offset.Y*w.PixelWidth + offset.X )];

            Color c = Color.FromArgb( (byte) ( ( colorAsInt >> 0x18 ) & 0xff ),
                                      (byte) ( ( colorAsInt >> 0x10 ) & 0xff ),
                                      (byte) ( ( colorAsInt >> 8 ) & 0xff ),
                                      (byte) ( colorAsInt & 0xff ) );

            _selectedColor = c;
        }
    }
}