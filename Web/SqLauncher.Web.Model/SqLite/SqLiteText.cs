// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteText.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 14 19:54
//   * Modified at: 2011  11 14  19:55
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   Represents the SqLite Text type.
    ///   The value is a text string, stored using the database encoding (UTF-8, UTF-16BE or UTF-16LE).
    /// </summary>
    public class SqLiteText : SqlTypeBase
    {
        /// <summary>
        ///   The string representation of the Text type.
        /// </summary>
        public const string TypeName = "Text";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.SqLite.SqLiteText" /> class.
        /// </summary>
        public SqLiteText()
            : base( TypeName, true )
        {
        }
    }
}