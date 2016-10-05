// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteDataModelGenerator.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 13 14:59
//   * Modified at: 2011  11 13  15:01
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   The sql generator for sql lite.
    /// </summary>
    public class SqLiteDataModelGenerator : DataModelGeneratorBase
    {
        /// <summary>
        /// The sqlite entity generator.
        /// </summary>
        private readonly SqLiteERDEntityGenerator _entityGenerator = new SqLiteERDEntityGenerator();

        /// <summary>
        /// The bath delimiter.
        /// </summary>
        public const string EntityDelimiter = ";";

        /// <summary>
        ///   Creates a sql statements by passed datamodel object.
        /// </summary>
        /// <param name = "dataModel">The data model.</param>
        /// <returns>The generated sql.</returns>
        public override string Generate( DataModel dataModel )
        {
            var result = new StringBuilder();

            var entities =
                new List<SqLitePositionContainer>();
           
            FillContainers( entities, dataModel );

            int i = 0;
            foreach ( var entity in entities ){
                result.AppendFormat( "{0}{1}", _entityGenerator.GenerateSql( entity.Entity ), EntityDelimiter);
                RiseERDEntityProcessed( entity.Entity );
                i++;
                if ( i<entities.Count ){
                    result.AppendLine();
                    result.AppendLine();
                } //if
            } //foreach
            
            return result.ToString();
        }

        /// <summary>
        /// Arranges the entities for generation DDL script.
        /// </summary>
        /// <param name="entities">The containers of arranged entities.</param>
        /// <param name="dataModel">The data model.</param>
        private void FillContainers( List<SqLitePositionContainer> entities, DataModel dataModel )
        {
            foreach ( var entity in dataModel.Entities ){
                var container = new SqLitePositionContainer{Entity = entity};
                entities.Add( container );
                container.Position = container.Entity.ChildRelations.ToList().Count + 1;
            } //foreach


            entities.Sort(
                ( cr1, cr2 ) => cr1.Position.CompareTo( cr2.Position ) );
        }
    }
}