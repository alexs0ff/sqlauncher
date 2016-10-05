// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RibbonColorButton.xaml.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 09 25 10:46 AM
//   * Modified at: 2011  10 03  9:32 PM
// / ******************************************************************************/ 


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace SqLauncher.Web.Ribbon
{
    [ContentProperty( "RibbonColorList" )]
    public partial class RibbonColorButton : StackPanel
    {
        public RibbonColorButton()
        {
            // Required to initialize variables
            InitializeComponent();
            // Attach an events
            Loaded += RibbonButtonLoaded;
            mainButton.Click += ButtonClick;
            mainButton.LostFocus += ButtonLostFocus;
            popup.Closed += PopupClosed;
            popup.Opened += PopupOpened;
        }

        #region ColorChanged 

        /// <summary>
        /// The old brush value.
        /// </summary>
        private Brush _oldBrush;

        /// <summary>
        /// Occurs when brush has been changed.
        /// </summary>
        public event EventHandler<BrushChangedEventArgs> BrushChanged;

        /// <summary>
        /// Rises the BrushChanged event.
        /// </summary>
        /// <param name="oldBrush">The old brush.</param>
        /// <param name="newBrush">The new brush.</param>
        private void RiseBrushChanged( Brush oldBrush, Brush newBrush )
        {
            EventHandler<BrushChangedEventArgs> handler = BrushChanged;
            if ( handler != null ){
                handler( this, new BrushChangedEventArgs( newBrush,oldBrush ) );
            }
        }

        /// <summary>
        /// Occurs when color pallete popup has been open.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void PopupOpened( object sender, EventArgs e )
        {
            _oldBrush = RibbonColorList.Brush;
        }

        /// <summary>
        /// Occurs the color pallete popup has been closed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void PopupClosed( object sender, EventArgs e )
        {
            RiseBrushChanged( _oldBrush, RibbonColorList.Brush );
        }

        #endregion ColorChanged

        private void RibbonButtonLoaded( object sender, RoutedEventArgs e )
        {
            if ( !_hasLoaded ){
                // create image
                if ( _image == null && ImageUrl != null && ImageUrl.ToString() != "" ){
                    _image = new Image();
                    _image.VerticalAlignment = VerticalAlignment.Top;
                    _image.HorizontalAlignment = HorizontalAlignment.Center;
                    _image.Stretch = Stretch.Uniform;
                    _image.SetValue( Image.SourceProperty, ImageUrl );
                    ContentPanel.Children.Add( _image );
                }

                // create list
                if ( RibbonColorList != null && !popupPanel.Children.Contains( RibbonColorList ) ){
                    popupPanel.Children.Add( RibbonColorList );

                    RibbonColorList.Popup = popup;
                    RibbonColorList.BrushChanged += RibbonColorListOnColorChanged;
                    //
                    _arrowImage = new Image();
                    _arrowImage.Height = 4;
                    _arrowImage.Width = 8;
                    _arrowImage.VerticalAlignment = VerticalAlignment.Top;
                    _arrowImage.HorizontalAlignment = HorizontalAlignment.Center;
                    _arrowImage.Stretch = Stretch.Uniform;
                    RibbonUtils.SetImage( _arrowImage, ArrowImageSource );
                    //ContentPanel.Children.Add( _arrowImage );
                }

                // create tooltip
                if ( TooltipTitle != "" || TooltipText != "" && ToolTipService.GetToolTip( this ) == null ){
                    TextBlock title = new TextBlock();
                    TextBlock tooltiptext = new TextBlock();

                    title.Text = TooltipTitle;
                    title.Style = tooltipTitleStyle;

                    tooltiptext.Text = TooltipText;
                    tooltiptext.Style = tooltipTextStyle;

                    StackPanel panel = new StackPanel();
                    panel.HorizontalAlignment = HorizontalAlignment.Stretch;
                    panel.Children.Add( title );
                    panel.Children.Add( tooltiptext );
                    //panel.Style = tooltipStyle2;                

                    ToolTip t = new ToolTip();
                    t.Style = tooltipStyle;
                    t.Content = panel;
                    ToolTipService.SetToolTip( this, t );
                }

                //if (this.button.Style == null)
                //    this.button.Style = buttonStyle;

                _hasLoaded = true;
            }
        }

        private void RibbonColorListOnColorChanged( object sender, EventArgs eventArgs )
        {
            bottomColorView.Background = RibbonColorList.Brush;
        }

        # region Events

        private bool _hasLoaded;

        private void ButtonClick( object sender, RoutedEventArgs e )
        {
            if ( RibbonColorList != null ){
                popup.IsOpen = !popup.IsOpen;
            }
            else{
                if ( OnClick != null ){
                    OnClick( this, e );
                }
            }
        }

        private void ButtonLostFocus( object sender, RoutedEventArgs e )
        {
            if ( RibbonColorList != null && popup.IsOpen ){
                popup.IsOpen = false;
            }
        }

        public void ArrangeInGroup()
        {
            mainButton.Height = mainButton.MaxHeight = this.RibbonItem.RIMain.Height/ParentGroup.VertButtonsCount;
            // если задана высота группы
            if ( ParentGroup.Height.ToString() != "NaN" ){
                mainButton.Height = ParentGroup.Height/ParentGroup.VertButtonsCount;
            }
            // если задана высота кнопки
            if ( Height.ToString() != "NaN" ){
                mainButton.Height = Height;
            }

            ContentPanel.Orientation = Orientation.Horizontal;

            _image.Width = mainButton.Height;
            if ( _image.Width > 11 ){
                _image.Width -= 11;
            }

            if ( _arrowImage != null ){
                _arrowImage.HorizontalAlignment = HorizontalAlignment.Left;
                _arrowImage.VerticalAlignment = VerticalAlignment.Center;
            }

            bottomColorView.Width = _image.Width + 4;
            //}            
        }

        # endregion

        # region Fields

        public event RoutedEventHandler OnClick;

        private const string ArrowImageSource = "img/arrowImage.png";

        private string _tooltipTitle = "";

        public string TooltipTitle
        {
            get { return _tooltipTitle; }
            set { _tooltipTitle = value; }
        }

        private string _tooltipText = "";

        public string TooltipText
        {
            get { return _tooltipText; }
            set { _tooltipText = value; }
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register( "Enabled", typeof ( bool ), typeof ( RibbonColorButton ),
                                                        new PropertyMetadata( true,EnabledChange ) );

        private static void EnabledChange( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var ribbonColorButton = d as RibbonColorButton;

            if ( ribbonColorButton!=null && ribbonColorButton.mainButton!=null ){
                ribbonColorButton.mainButton.IsEnabled = ribbonColorButton.Enabled = (bool)e.NewValue;
            } //if
        }

        public bool Enabled
        {
            get { return (bool) GetValue( EnabledProperty ); }
            set { SetValue( EnabledProperty, value ); }
        }

        private ImageSource _imageUrl;

        public ImageSource ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        private Image _image;

        private Image _arrowImage;

        public RibbonItem RibbonItem { get; internal set; }

        public RibbonButtonsGroup ParentGroup { get; internal set; }

        public RibbonColorList RibbonColorList { get; set; }

        #endregion
    }

    public class ColorEventArgs
    {
        public ColorEventArgs( Color color )
        {
            this.Color = color;
        }

        public Color Color;
    }
}