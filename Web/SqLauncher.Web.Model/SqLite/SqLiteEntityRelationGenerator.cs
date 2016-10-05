// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteEntityRelationGenerator.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 17 20:40
//   * Modified at: 2011  11 17  20:41
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Text;

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   The sql references generator.
    /// </summary>
    public class SqLiteEntityRelationGenerator : EntityRelationGeneratorBase
    {
        /// <summary>
        ///   The Foreign Key.
        /// </summary>
        public const string ForeignKey = "FOREIGN KEY";

        /// <summary>
        ///   The References word.
        /// </summary>
        public const string References = "REFERENCES";

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
        ///   Generates the DDL string that represents the passed object.
        /// </summary>
        /// <param name = "modelObject">The model object for sql creating.</param>
        /// <returns>The created sql.</returns>
        public override string GenerateSql( EntityRelation modelObject )
        {
            var result = new StringBuilder();

            result.AppendFormat( "{0} {1}", ForeignKey, OpenBracket );

            var attributes = modelObject.ChildAttributes;

            AppendAttributes( result, attributes );

            result.Append( CloseBracket );

            result.AppendFormat( " {0} {1}{2}", References, modelObject.Parent.Caption.Physical, OpenBracket );

            attributes = modelObject.ParentAttributes;
            AppendAttributes( result, attributes );

            result.Append( CloseBracket );

            return result.ToString();
        }

        /// <summary>
        ///   Appends the attributes.
        /// </summary>
        /// <param name = "result">The string builder.</param>
        /// <param name = "attributes">The attributes.</param>
        private static void AppendAttributes( StringBuilder result, ICollection<EntityAttribute> attributes )
        {
            var i = 1;
            foreach ( var attribute in attributes ){
                result.Append( attribute.Caption.Physical );

                if ( attributes.Count > i ){
                    result.Append( Comma );
                } //if

                i++;
            } //foreach
        }
    }
}