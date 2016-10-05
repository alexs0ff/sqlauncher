// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteTypesMapper.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 14 20:08
//   * Modified at: 2011  11 14  20:13
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   The mapper for sqlite types.
    /// </summary>
    public class SqLiteTypesMapper : IDbTypesMapper
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.SqLite.SqLiteTypesMapper" /> class.
        /// </summary>
        public SqLiteTypesMapper()
        {
            _registeredTypes = new Collection<SqlTypeBase>{
                                                              new SqLiteInteger(),
                                                              new SqLiteBlob(),
                                                              new SqLiteReal(),
                                                              new SqLiteText()
                                                          };
        }

        /// <summary>
        ///   The registred types.
        /// </summary>
        private readonly IList<SqlTypeBase> _registeredTypes;

        /// <summary>
        ///   The registred types.
        /// </summary>
        public ICollection<SqlTypeBase> RegisteredTypes
        {
            get { return new ReadOnlyCollection<SqlTypeBase>( _registeredTypes ); }
        }
    }
}