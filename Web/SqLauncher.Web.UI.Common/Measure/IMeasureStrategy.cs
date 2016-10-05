// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IMeasureStrategy.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 08 5:06 PM
//   * Modified at: 2011  10 08  5:15 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.UI.Common.Measure
{
    /// <summary>
    ///   Represents a startegy for all kind of measuring.
    /// </summary>
    public interface IMeasureStrategy
    {
        /// <summary>
        ///   The name of unit.
        /// </summary>
        string UnitName { get; }

        /// <summary>
        ///   Converts from the Device Independent Units to specific measure unit.
        /// </summary>
        /// <param name = "value">The DIU value.</param>
        /// <returns>The converted value..</returns>
        double Convert( double value );

        /// <summary>
        ///   Converts to the Device Independent Units from specific measure unit.
        /// </summary>
        /// <param name = "value">The value to convert.</param>
        /// <returns>The DIU value</returns>
        double ConvertBack( double value );
    }
}