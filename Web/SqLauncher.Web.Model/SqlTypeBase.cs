// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqlTypeBase.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 14 19:54
//   * Modified at: 2011  11 14  19:55
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Represents the base type for all SqlLite storage classes:
    /// </summary>
    public abstract class SqlTypeBase
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.SqlTypeBase" /> class.
        /// </summary>
        /// <param name = "typeName">The string representetion of the current type.</param>
        /// <param name = "hasLenght"></param>
        protected SqlTypeBase(string typeName, bool hasLenght)
            : this(typeName, hasLenght, false)
        {

        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.SqlTypeBase" /> class.
        /// </summary>
        /// <param name = "typeName">The string representetion of the current type.</param>
        /// <param name = "hasLenght"></param>
        /// <param name="hasDecimal"></param>
        protected SqlTypeBase(string typeName, bool hasLenght,bool hasDecimal)
        {
            Name = typeName;
            HasLenght = hasLenght;
            HasDecimal = hasDecimal;
        }

        /// <summary>
        ///   The name of type, real, integer etc.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///   The flag which indicates that the type has lenght.
        /// </summary>
        public bool HasLenght { get; private set; }

        /// <summary>
        ///   The flag which indicates that the type has decimal digits.
        /// </summary>
        public bool HasDecimal { get; private set; }

        /// <summary>
        ///   Determines whether the specified <see cref = "T:SqLauncher.Web.Model.SqlTypeBase" /> is equal to the current <see
        ///    cref = "T:SqLauncher.Web.Model.SqlTypeBase" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref = "T:SqLauncher.Web.Model.SqlTypeBase" /> is equal to the current <see
        ///    cref = "T:SqLauncher.Web.Model.SqlTypeBase" />; otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "T:SqLauncher.Web.Model.SqlTypeBase" /> to compare with the current <see
        ///    cref = "T:SqLauncher.Web.Model.SqlTypeBase" />. </param>
        public override bool Equals( object obj )
        {
            var typeObj = obj as SqlTypeBase;

            if ( typeObj == null ){
                return false;
            }

            return Name.Equals( typeObj.Name );
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref = "T:System.Object" />.
        /// </returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}