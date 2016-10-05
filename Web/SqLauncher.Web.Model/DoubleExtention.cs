// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DoubleExtention.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 09 02 22:26
//   * Modified at: 2011  11 16  20:57
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Extention for the double type.
    /// </summary>
    public static class DoubleExtention
    {
        /// <summary>
        ///   extension method in  for comparing double values.
        /// </summary>
        /// <param name = "double1"></param>
        /// <param name = "double2"></param>
        /// <param name = "precision"></param>
        /// <returns></returns>
        public static bool AlmostEquals( this double double1, double double2, double precision )
        {
            return ( Math.Abs( double1 - double2 ) <= precision );
        }
    }
}