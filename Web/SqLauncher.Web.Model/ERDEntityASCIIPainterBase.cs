// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ERDEntityASCIIPainterBase.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2012 03 11 21:47
//   * Modified at: 2012  03 11  21:49
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Represents the ascii painter for ERD Entity object.
    /// </summary>
    public abstract class ERDEntityASCIIPainterBase : IASCIIPainter<ERDEntity>
    {
        /// <summary>
        ///   Generates the ASCII symbols that represents the passed object.
        /// </summary>
        /// <param name = "modelObject">The model object for ascii creating.</param>
        /// <returns>The created text.</returns>
        public abstract string GenerateText( ERDEntity modelObject );
    }
}