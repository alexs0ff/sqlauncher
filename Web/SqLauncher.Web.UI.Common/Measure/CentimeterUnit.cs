// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   CentimeterUnit.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 08 5:27 PM
//   * Modified at: 2011  10 08  5:28 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.UI.Common.Measure
{
    /// <summary>
    ///   The convertor to centimeter measure.
    /// </summary>
    public class CentimeterUnit : IMeasureStrategy
    {
        /// <summary>
        ///   The name of this strategy.
        /// </summary>
        public const string Name = "Centimeter";

        /// <summary>
        ///   The factor of measuring.
        /// </summary>
        private const double Factor = 1/( 96*2.54 );

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
        /// <returns>The centimeter value.</returns>
        public double Convert( double value )
        {
            return value*Factor;
        }

        /// <summary>
        ///   Converts to the Device Independent Units from specific measure unit.
        /// </summary>
        /// <param name = "value">The centimeter value to convert.</param>
        /// <returns>The DIU value</returns>
        public double ConvertBack( double value )
        {
            return value/Factor;
        }
    }
}