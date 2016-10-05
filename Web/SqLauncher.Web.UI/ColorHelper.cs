// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ColorHelper.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 10 02 10:23 AM
//   * Modified at: 2011  10 02  10:28 AM
// / ******************************************************************************/ 

using System.Windows.Media;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   The helper for color handling purposes.
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        ///   Parses the string which contains the ARGB color raw data etc "#FFFFFFFF".
        ///   if string has wrong format then method throws the FormatException.
        /// </summary>
        /// <param name = "colorString">The string to parse.</param>
        /// <returns>The color instance.</returns>
        public static Color FromString( string colorString )
        {
            colorString = colorString.Replace( "#", "" );
            byte a = System.Convert.ToByte( "ff", 16 );

            byte pos = 0;
            if (colorString.Length == 8)
            {
                a = System.Convert.ToByte(colorString.Substring(pos, 2), 16);

                pos = 2;
            }

            byte r = System.Convert.ToByte(colorString.Substring(pos, 2), 16);
            pos += 2;

            byte g = System.Convert.ToByte(colorString.Substring(pos, 2), 16);
            pos += 2;

            byte b = System.Convert.ToByte(colorString.Substring(pos, 2), 16);

            return Color.FromArgb( a, r, g, b );
        }
    }
}