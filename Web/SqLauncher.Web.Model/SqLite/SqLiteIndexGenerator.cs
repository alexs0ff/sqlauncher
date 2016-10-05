// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteIndexGenerator.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2012 02 24 20:10
//   * Modified at: 2012  02 24  20:40
// / ******************************************************************************/ 

using System.Text;

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   The sqlite entity index generator.
    /// </summary>
    public class SqLiteIndexGenerator : IndexGeneratorBase
    {
        /// <summary>
        ///   The create word.
        /// </summary>
        public const string Create = "CREATE";

        /// <summary>
        ///   The index word.
        /// </summary>
        public const string Index = "INDEX";

        /// <summary>
        ///   The unique word.
        /// </summary>
        public const string Unique = "UNIQUE";

        /// <summary>
        ///   The on word.
        /// </summary>
        public const string On = "ON";

        /// <summary>
        ///   The desc word.
        /// </summary>
        public const string Desc = "desc";

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
        public override string GenerateSql( EntityIndex modelObject )
        {
            if ( modelObject.Attributes.Count == 0 ){
                return string.Empty;
            } //if

            var result = new StringBuilder();

            if ( modelObject.IsUnique ){
                result.AppendFormat( "{0} {1} {2} {3} {4} {5}", Create, Unique, Index, modelObject.Caption.Physical, On,
                                     modelObject.Parent.Caption.Physical );
            } //if
            else{
                result.AppendFormat( "{0} {1} {2} {3} {4}", Create, Index, modelObject.Caption.Physical, On,
                                     modelObject.Parent.Caption.Physical );
            } //else

            result.Append( OpenBracket );

            int current = 0;
            foreach ( var indexAttribute in modelObject.Attributes ){
                current++;

                if ( current > 1 ){
                    result.Append( " " );
                } //if

                if ( indexAttribute.Order == SortOrder.Descending ){
                    result.AppendFormat( "{0} {1}", indexAttribute.Attribute.Caption.Physical, Desc );
                } //if
                else{
                    result.AppendFormat( "{0}", indexAttribute.Attribute.Caption.Physical );
                } //else
                if ( current < modelObject.Attributes.Count ){
                    result.Append( Comma );
                } //if
            } //foreach

            result.Append( CloseBracket );

            return result.ToString();
        }
    }
}