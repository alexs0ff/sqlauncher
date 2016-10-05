// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteInteger.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 17 9:01 PM
//   * Modified at: 2011  08 22  4:26 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   Represents the INTEGER type of SqLiteDatabases.
    /// </summary>
    public class SqLiteInteger : SqlTypeBase
    {
        /// <summary>
        ///   The string representation of the integer type.
        /// </summary>
        public const string TypeName = "Integer";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.SqLite.SqLiteInteger" /> class.
        /// </summary>
        public SqLiteInteger() : base( TypeName, false )
        {
        }
    }
}