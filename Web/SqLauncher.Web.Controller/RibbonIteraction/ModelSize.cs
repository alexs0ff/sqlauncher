// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ModelSize.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 10 08 6:09 PM
//   * Modified at: 2011  10 08  6:22 PM
// / ******************************************************************************/ 

using System.Windows;

using SqLauncher.Web.UI.Common.Measure;

namespace SqLauncher.Web.Controller.RibbonIteraction
{
    /// <summary>
    ///   The ribbon unit for model size adjustment.
    /// </summary>
    public class ModelSize : DependencyObject
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.RibbonIteraction.ModelSize" /> class.
        /// </summary>
        public ModelSize()
        {
            Measure = PixelUnit.Name;
            Width = new MeasureProxy();
            Height = new MeasureProxy();
        }

        /// <summary>
        ///   The model width.
        /// </summary>
        public MeasureProxy Width { get; set; }

        /// <summary>
        ///   The model height.
        /// </summary>
        public MeasureProxy Height { get; set; }

        public static readonly DependencyProperty MeasureProperty =
            DependencyProperty.Register( "Measure", typeof ( string ), typeof ( ModelSize ),
                                         new PropertyMetadata( OnMeasureChange ) );

        private static void OnMeasureChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var modelSize = (ModelSize) d;

            if (modelSize.Width != null && modelSize.Height!=null)
            {
                modelSize.Width.Measure = (string)e.NewValue;
                modelSize.Height.Measure = (string)e.NewValue;
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
    }
}