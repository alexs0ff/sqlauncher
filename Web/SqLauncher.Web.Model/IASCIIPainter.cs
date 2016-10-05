// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IASCIIPainter.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2012 03 11 20:28
//   * Modified at: 2012  03 11  20:36
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Represents the interface for ASCII symbols which paint a model object.
    /// </summary>
    public interface IASCIIPainter<T> where T : ModelObject
    {
        /// <summary>
        ///   Generates the ASCII that represents the passed object.
        /// </summary>
        /// <param name = "modelObject">The model object for ascii creating.</param>
        /// <returns>The created text.</returns>
        string GenerateText( T modelObject );
    }
}