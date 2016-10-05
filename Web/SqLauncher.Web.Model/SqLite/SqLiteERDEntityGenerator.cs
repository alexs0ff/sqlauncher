// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteERDEntityGenerator.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2011  11 21  20:14
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   The sql lite erd entity generator.
    /// </summary>
    public class SqLiteERDEntityGenerator : ERDEntityGeneratorBase
    {
        /// <summary>
        ///   The relations generator.
        /// </summary>
        private readonly SqLiteEntityRelationGenerator _entityRelationGenerator = new SqLiteEntityRelationGenerator();

        /// <summary>
        ///   The attribute generator.
        /// </summary>
        private readonly SqLiteEntityAttributeGenerator _sqLiteEntityAttributeGenerator = new SqLiteEntityAttributeGenerator();

        /// <summary>
        /// The index generator.
        /// </summary>
        private readonly SqLiteIndexGenerator _sqLiteIndexGenerator = new SqLiteIndexGenerator();

        /// <summary>
        ///   The create word.
        /// </summary>
        public const string Create = "CREATE";

        /// <summary>
        ///   The table word.
        /// </summary>
        public const string Table = "TABLE";

        /// <summary>
        ///   The open bracket.
        /// </summary>
        public const string OpenBracket = "(";

        /// <summary>
        ///   The close bracket.
        /// </summary>
        public const string CloseBracket = ")";

        /// <summary>
        ///   The comma.
        /// </summary>
        public const string Comma = ",";

        /// <summary>
        /// The bath delimiter.
        /// </summary>
        public const string Delimiter = ";";

        /// <summary>
        ///   Generates the DDL string that represents the passed object.
        /// </summary>
        /// <param name = "modelObject">The model object for sql creating.</param>
        /// <returns>The created sql.</returns>
        public override string GenerateSql( ERDEntity modelObject )
        {
            var result = new StringBuilder();

            result.AppendFormat( "{0} {1} {2}", Create, Table, modelObject.Caption.Physical );
            result.AppendLine();
            result.AppendLine( OpenBracket );

            var i = 1;

            foreach ( var attribute in modelObject ){
                result.AppendFormat("\t{0}", _sqLiteEntityAttributeGenerator.GenerateSql(attribute));

                if ( i < modelObject.Attributes.Count ){
                    result.Append( Comma );
                    result.AppendLine();
                } //if

                i++;
            } //foreach

            GenerateRelations( modelObject, result );

            result.Append( CloseBracket );

            //process indexes
            foreach ( var index in modelObject.Indexes){
                result.AppendLine( Delimiter );
                result.Append( _sqLiteIndexGenerator.GenerateSql( index ) );

            } //foreach

            return result.ToString();
        }

        /// <summary>
        /// Generates the depended repations.
        /// </summary>
        /// <param name="modelObject">The model object.</param>
        /// <param name="result">The string builder.</param>
        private void GenerateRelations( ERDEntity modelObject, StringBuilder result )
        {
            //process only child relations!!
            if ( modelObject.ChildRelations.ToList().Count > 0 ){
                result.Append( Comma );
                GenerateRelationSql( modelObject.ChildRelations.ToList(), result );
            } //if
            else{
                result.AppendLine();
            } //else
        }

        /// <summary>
        ///   Generates the relation sql block.
        /// </summary>
        /// <param name = "relations">The collection fo entity relation element.</param>
        /// <param name = "result">The StringBuilder buffer.</param>
        private void GenerateRelationSql( ICollection<EntityRelation> relations, StringBuilder result )
        {
            int i = 0;
            result.AppendLine();

            foreach ( var entityRelation in relations ){
                result.AppendFormat( "\t{0}", _entityRelationGenerator.GenerateSql( entityRelation ) );

                i++;
                if ( i < relations.Count ){
                    result.Append( Comma );
                } //if
                result.AppendLine();
            } //foreach
        }
    }
}