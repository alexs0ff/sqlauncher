// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   PixelUnit.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 08 5:35 PM
//   * Modified at: 2011  10 08  5:37 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.UI.Common.Measure
{
    /// <summary>
    ///   The convertor to pixel measure.
    ///   we are means that DIU = pixel value.
    /// </summary>
    public class PixelUnit : IMeasureStrategy
    {
        /// <summary>
        ///   The name of this strategy.
        /// </summary>
        public static string Name = "Pixel";

        /// <summary>
        ///   The name of unit.
        /// </summary>
        public string UnitName
        {
            get { return Name; }
        }

        /// <summary>
        ///   Converts from the Device Independent Units to specific measure unit.
        /// </summary>
        /// <param name = "value">The DIU value.</param>
        /// <returns>The pixel value.</returns>
        public double Convert( double value )
        {
            return value;
        }

        /// <summary>
        ///   Converts to the Device Independent Units from specific measure unit.
        /// </summary>
        /// <param name = "value">The pixel value to convert.</param>
        /// <returns>The DIU value</returns>
        public double ConvertBack( double value )
        {
            return value;
        }
    }
}