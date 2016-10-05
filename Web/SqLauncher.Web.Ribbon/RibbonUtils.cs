// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RibbonUtils.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 09 25 10:46 AM
//   * Modified at: 2011  09 25  10:56 AM
// / ******************************************************************************/ 

using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;

namespace SqLauncher.Web.Ribbon
{
    public class RibbonUtils
    {
        public static void SetImage( Image imageSource, string imageString )
        {
            Uri uri = new Uri( imageString, UriKind.Relative );
            ImageSource imgSrc = new System.Windows.Media.Imaging.BitmapImage( uri );
            imageSource.Source = imgSrc;
        }

        public static ImageSource GetImageSource( string imageString )
        {
            Uri uri = new Uri( imageString, UriKind.Relative );
            return new System.Windows.Media.Imaging.BitmapImage( uri );
        }

        public static Color ColorFromString( string colorString )
        {
            Type colorType = ( typeof ( System.Windows.Media.Colors ) );
            if ( colorType.GetProperty( colorString ) != null ){
                object color = colorType.InvokeMember( colorString, BindingFlags.GetProperty, null, null, null );
                if ( color != null ){
                    return (Color) color;
                }
                else{
                    return Colors.Black;
                }
            }
            else{
                return Colors.Black;
            }
        }

        public static Color ColorFromArgb( string argbString )
        {
            Color color = Colors.Black;
            Char[] chars = argbString.ToCharArray();
            if ( chars.Length == 9 ){
                Byte a = Convert.ToByte( Convert.ToInt32( chars[1].ToString() + chars[2].ToString(), 16 ) );
                Byte r = Convert.ToByte( Convert.ToInt32( chars[3].ToString() + chars[4].ToString(), 16 ) );
                Byte g = Convert.ToByte( Convert.ToInt32( chars[5].ToString() + chars[6].ToString(), 16 ) );
                Byte b = Convert.ToByte( Convert.ToInt32( chars[7].ToString() + chars[8].ToString(), 16 ) );
                color = Color.FromArgb( a, r, g, b );
            }
            return color;
        }
    }
}