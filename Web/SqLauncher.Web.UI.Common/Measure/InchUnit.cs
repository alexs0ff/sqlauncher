// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   InchUnit.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 08 5:17 PM
//   * Modified at: 2011  10 08  5:21 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.UI.Common.Measure
{
    /// <summary>
    /// The convertor to inch measure.
    /// </summary>
    public class InchUnit : IMeasureStrategy
    {
        /// <summary>
        ///   The name of this strategy.
        /// </summary>
        public const string Name = "Inch";

        /// <summary>
        ///   The factor of measuring.
        /// </summary>
        private const double Factor = 1.0/96;

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
        /// <returns>The inch value.</returns>
        public double Convert( double value )
        {
            return value*Factor;
        }

        /// <summary>
        ///   Converts to the Device Independent Units from specific measure unit.
        /// </summary>
        /// <param name = "value">The inch value to convert.</param>
        /// <returns>The DIU value</returns>
        public double ConvertBack( double value )
        {
            return value/Factor;
        }
    }
}