// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   StringExtentions.cs
//   * Project: SqLauncher.Web.Model
//   * Description: The String class extentions.
//   * Created at 2011 08 18 11:11 PM
//   * Modified at: 2011  08 18  11:15 PM
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Contains a extentions of String class.
    /// </summary>
    public static class StringExtentions
    {
        /// <summary>
        ///   Compares two string of their relative sort order.
        /// </summary>
        /// <param name = "value"></param>
        /// <param name = "target">The target string.</param>
        /// <returns>True if equal otherwise false.</returns>
        public static bool OrdinalIgnoreCaseEqual( this string value, string target )
        {
            if ( value == null || target == null ){
                return false;
            }
            return StringComparer.OrdinalIgnoreCase.Compare( value, target ) == 0;
        }
    }
}