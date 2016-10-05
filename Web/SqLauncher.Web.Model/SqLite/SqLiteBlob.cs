// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteBlob.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 17 9:08 PM
//   * Modified at: 2011  08 22  4:27 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   Represents the BLOB type.
    /// </summary>
    public class SqLiteBlob : SqlTypeBase
    {
        /// <summary>
        ///   The string representation of the blob type.
        /// </summary>
        public const string TypeName = "Blob";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.SqLite.SqLiteBlob" /> class.
        /// </summary>
        public SqLiteBlob()
            : base( TypeName, true )
        {
        }
    }
}