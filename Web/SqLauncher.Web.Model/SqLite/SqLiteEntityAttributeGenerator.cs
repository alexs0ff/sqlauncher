// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteEntityAttributeGenerator.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 15 20:09
//   * Modified at: 2011  11 15  20:41
// / ******************************************************************************/ 

using System.Text;

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   Represents the attribute generator for sqlite database.
    /// </summary>
    public class SqLiteEntityAttributeGenerator : EntityAttributeGeneratorBase
    {
        /// <summary>
        ///   The PRIMARY KEY constaint name.
        /// </summary>
        public const string PrimaryKey = "PRIMARY KEY";

        /// <summary>
        ///   The UNIQUE constaint name.
        /// </summary>
        public const string Unique = "UNIQUE";

        /// <summary>
        ///   The NOT NULL constaint name.
        /// </summary>
        public const string NotNull = "NOT NULL";

        /// <summary>
        ///   The AUTOINCREMENT defenition name.
        /// </summary>
        public const string AutoIncrement = "AUTOINCREMENT";

        /// <summary>
        ///   Generates the DDL string that represents the passed object.
        /// </summary>
        /// <param name = "modelObject">The model object for sql creating.</param>
        /// <returns>The created sql.</returns>
        public override string GenerateSql( EntityAttribute modelObject )
        {
            var result = new StringBuilder();

            result.AppendFormat( "{0} ", modelObject.Caption.Physical );

            if ( modelObject.DbType.HasLenght ){
                result.AppendFormat( "{0}({1})", modelObject.DbType.Name, modelObject.DataLenght );
            } //if
            else{
                result.Append( modelObject.DbType.Name );
            } //else

            if ( modelObject.Key== AttributeKeyType.IsKey ){
                result.AppendFormat( " {0}", PrimaryKey );

                if ( modelObject.IsIdentity ){
                    result.AppendFormat( " {0}", AutoIncrement );
                } //if
            } //if

            if ( modelObject.IsUnique ){
                result.AppendFormat( " {0}", Unique );
            } //if

            if ( modelObject.IsNotNull ){
                result.AppendFormat( " {0}", NotNull );
            } //if


            return result.ToString();
        }
    }
}