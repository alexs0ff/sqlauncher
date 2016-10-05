// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityRelationGeneratorBase.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 13 13:44
//   * Modified at: 2011  11 13  13:44
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Text;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   The sql generator for EntityRelation.
    /// </summary>
    public abstract class EntityRelationGeneratorBase : ISqlGenerator<EntityRelation>
    {
        /// <summary>
        ///   Generates the DDL string that represents the passed object.
        /// </summary>
        /// <param name = "modelObject">The model object for sql creating.</param>
        /// <returns>The created sql.</returns>
        public abstract string GenerateSql( EntityRelation modelObject );
    }
}