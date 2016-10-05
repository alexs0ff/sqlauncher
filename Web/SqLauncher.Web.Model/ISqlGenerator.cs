// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ISqlGenerator.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 13 13:28
//   * Modified at: 2011  11 13  13:35
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Represents the interface for sql generating all model objects.
    /// </summary>
    public interface ISqlGenerator<T> where T : ModelObject
    {
        /// <summary>
        ///   Generates the DDL string that represents the passed object.
        /// </summary>
        /// <param name = "modelObject">The model object for sql creating.</param>
        /// <returns>The created sql.</returns>
        string GenerateSql( T modelObject );
    }
}