// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IndexGeneratorBase.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2012 02 24 20:12
//   * Modified at: 2012  02 24  20:13
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   The sql generator for Entity index.
    /// </summary>
    public abstract class IndexGeneratorBase : ISqlGenerator<EntityIndex>
    {
        /// <summary>
        ///   Generates the DDL string that represents the passed object.
        /// </summary>
        /// <param name = "modelObject">The model object for sql creating.</param>
        /// <returns>The created sql.</returns>
        public abstract string GenerateSql( EntityIndex modelObject );
    }
}