// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteReal.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 14 19:54
//   * Modified at: 2011  11 14  19:55
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   Represents the SqLite type REAL.
    ///   The value is a floating point value, stored as an 8-byte IEEE floating point number.
    /// </summary>
    public class SqLiteReal : SqlTypeBase
    {
        /// <summary>
        ///   The string representation of the Real type.
        /// </summary>
        public const string TypeName = "Real";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.SqLite.SqLiteReal" /> class.
        /// </summary>
        public SqLiteReal()
            : base( TypeName, false )
        {
        }
    }
}