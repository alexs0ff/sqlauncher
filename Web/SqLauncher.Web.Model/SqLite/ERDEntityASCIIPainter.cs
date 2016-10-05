// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ERDEntityASCIIPainter.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2012 03 11 21:49
//   * Modified at: 2012  03 11  22:53
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   The painter for ASCII erd entity.
    ///   The system independed char subsets information: 
    ///   http://msdn.microsoft.com/en-us/library/ff806365%28v=vs.95%29.aspx 
    ///   http://www.apaddedcell.com/web-fonts
    /// </summary>
    public class ERDEntityASCIIPainter : ERDEntityASCIIPainterBase
    {
        private const char AngleChar = '+';

        private const char TopAndButtonChar = '-';

        private const char LeftAndRightChar = '|';

        private const char Space = ' ';

        /// <summary>
        /// Gets the lenghth of string.
        /// Checks string to null values.
        /// </summary>
        /// <param name="value">The checked </param>
        /// <returns>The lenghth. 0 if null.</returns>
        private static int GetStringLengthSafe(string value)
        {
            if ( string.IsNullOrEmpty( value ) ){
                return 0;
            } //if

            return value.Length;
        }

        /// <summary>
        ///   Generates the ASCII symbols that represents the passed object.
        /// </summary>
        /// <param name = "modelObject">The model object for ascii creating.</param>
        /// <returns>The created text.</returns>
        public override string GenerateText( ERDEntity modelObject )
        {
            var result = new StringBuilder();

            var lines = new string[modelObject.Attributes.Count];

            bool hasKeyAttribute = modelObject.Attributes.Any( attribute => attribute.Key != AttributeKeyType.None );

            for ( int i = 0; i < modelObject.Attributes.Count; i++ ){
                lines[i] = GenerateAtributeText( modelObject.Attributes.ToList()[i], hasKeyAttribute );
            } //for

            var maxCharsCount = DrawASCIICaption( modelObject, result, lines );
            result.AppendFormat("{0}{1}{2}", AngleChar, new String(TopAndButtonChar, maxCharsCount), AngleChar);
            result.AppendLine();

            foreach ( var line in lines ){
                result.AppendFormat( "{0}{1}{2}{3}", LeftAndRightChar, line,
                                     new String( Space, maxCharsCount - GetStringLengthSafe( line ) ), LeftAndRightChar );
                result.AppendLine();
            } //foreach

            //draw last line
            result.AppendFormat("{0}{1}{2}", AngleChar, new String(TopAndButtonChar, maxCharsCount), AngleChar);

            return result.ToString();
        }

        private static int DrawASCIICaption( ERDEntity modelObject, StringBuilder result, string[] lines )
        {
            var maxCharsCount = 0;

            if ( lines.Length > 0 ){
                maxCharsCount = lines.Max( s => GetStringLengthSafe( s ) );
            } //if

            if (maxCharsCount<GetStringLengthSafe(modelObject.Caption.Physical)){
                maxCharsCount = GetStringLengthSafe( modelObject.Caption.Physical );
            } //if

            //draw first line
            result.AppendFormat("{0}{1}{2}", AngleChar, new String(TopAndButtonChar, maxCharsCount), AngleChar);
            result.AppendLine();
            //draw caption
            int leftSpaceCount;

            int rightSpaceCount;
            int spaceDiff = maxCharsCount - GetStringLengthSafe( modelObject.Caption.Physical );

            if (spaceDiff % 2 == 0)
            {
                leftSpaceCount = rightSpaceCount = (spaceDiff) / 2;
            } //if
            else{
                leftSpaceCount = (spaceDiff + 1) / 2;
                rightSpaceCount = leftSpaceCount - 1;
            } //else

            //caption center on horizontal
            result.AppendFormat( "{0}{1}{2}{3}{4}", LeftAndRightChar,
                                 new String( Space, leftSpaceCount ), modelObject.Caption.Physical,
                                 new String(Space, rightSpaceCount), LeftAndRightChar);
            result.AppendLine();
            return maxCharsCount;
        }

        private const string PrimaryForeignKey = "(PFK) ";

        private const string ForeignKey = "(FK) ";

        private const string PrimaryKey = "(PK) ";

        private const string IsUnique = "Unique";

        private const string IsIdentity = "Autoincrement";

        private const string IsNotNull = "Not null";

        private const int MaxKeyInformationCount = 5;//PrimaryForeignKey.Length

        /// <summary>
        ///   The attribute keys string representations map.
        /// </summary>
        private readonly Dictionary<AttributeKeyType, string>
            _keysStrings
                = new Dictionary<AttributeKeyType, string>{
                                                              {AttributeKeyType.None, string.Empty},
                                                              {AttributeKeyType.IsForeignKey, ForeignKey},
                                                              {
                                                                  AttributeKeyType.IsPrimaryForeignKey,
                                                                  PrimaryForeignKey
                                                                  },
                                                              {AttributeKeyType.IsKey, PrimaryKey},
                                                          };

        /// <summary>
        ///   Generates the string wich describes an entity attribute.
        /// </summary>
        /// <param name = "attribute">The entity attribute.</param>
        /// <param name="hasKeyAttribute">The flag that endicates model object has attrubute different from Key.None</param>
        /// <returns>The generated line.</returns>
        private string GenerateAtributeText( EntityAttribute attribute, bool hasKeyAttribute )
        {
            var result = new StringBuilder();

            if ( hasKeyAttribute ){
                result.Append( new String( Space, MaxKeyInformationCount - _keysStrings[attribute.Key].Length ) );
            } //if

            result.Append( _keysStrings[attribute.Key] );
            result.AppendFormat( "{0} ", attribute.Caption.Physical );

            if ( attribute.DbType.HasLenght && !attribute.DbType.HasDecimal ){
                result.AppendFormat( "{0}({1}) ", attribute.DbType.Name, attribute.DataLenght );
            } //if

            if ( attribute.DbType.HasDecimal ){
                result.AppendFormat( "{0}({1},{2}) ", attribute.DbType.Name, attribute.DataLenght, attribute.Decimal );
            } //else

            if ( !attribute.DbType.HasLenght ){
                result.AppendFormat( "{0} ", attribute.DbType.Name );
            } //if

            if ( attribute.IsNotNull ){
                result.AppendFormat( "{0} ", IsNotNull );
            } //if

            if ( attribute.IsIdentity && attribute.Key != AttributeKeyType.IsForeignKey){
                result.AppendFormat( "{0} ", IsIdentity );
            } //if

            if ( attribute.IsUnique ){
                result.AppendFormat( "{0} ", IsUnique );
            } //if

            return result.ToString();
        }
    }
}