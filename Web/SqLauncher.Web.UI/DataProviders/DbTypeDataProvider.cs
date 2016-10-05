// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DbTypeDataProvider.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 27 9:12 PM
//   * Modified at: 2011  08 27  9:21 PM
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.SqLite;

namespace SqLauncher.Web.UI.DataProviders
{
    /// <summary>
    ///   The data provider of all Registered db types.
    /// </summary>
    public class DbTypeDataProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.UI.DataProviders.DbTypeDataProvider"/> class.
        /// </summary>
        public DbTypeDataProvider()
        {
            RegisteredTypes = GetRegisteredTypes();
        }

        public ICollection<SqlTypeBase> RegisteredTypes { get; private set; }

        /// <summary>
        ///   Returns the registered type instances.
        /// </summary>
        /// <returns>The collection of type instances.</returns>
        private static ICollection<SqlTypeBase> GetRegisteredTypes()
        {
            var result = new Collection<SqlTypeBase>{
                                                                     new SqLiteBlob(),
                                                                     new SqLiteInteger(),
                                                                     new SqLiteReal(),
                                                                     new SqLiteText()
                                                                 };


            return result.OrderBy( m => m.Name ).ToList();
        }
    }
}