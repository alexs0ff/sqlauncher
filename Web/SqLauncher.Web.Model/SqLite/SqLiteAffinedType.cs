// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteAffinedType.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 14 19:54
//   * Modified at: 2011  11 14  19:54
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   Represents a base type of all affined types.
    /// </summary>
    public class SqLiteAffinedType : SqlTypeBase
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.SqLite.SqLiteAffinedType" /> class.
        /// </summary>
        /// <param name = "typeName">The string representetion of the current type.</param>
        /// <param name = "hasLenght"></param>
        public SqLiteAffinedType(string typeName, bool hasLenght)
            : base(typeName, hasLenght)
        {

        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.SqLite.SqLiteAffinedType" /> class.
        /// </summary>
        /// <param name = "typeName">The string representetion of the current type.</param>
        /// <param name = "hasLenght"></param>
        /// <param name="hasDecimal"></param>
        public SqLiteAffinedType( string typeName, bool hasLenght, bool hasDecimal )
            : base( typeName, hasLenght, hasDecimal )
        {
        }
    }
}