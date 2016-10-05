// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RelationshipTypesDataProvider.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 10 16 5:49 PM
//   * Modified at: 2011  10 16  6:04 PM
// / ******************************************************************************/ 

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.DataProviders
{
    /// <summary>
    ///   The data provider for relationship types.
    /// </summary>
    public class RelationshipTypesDataProvider
    {
        static RelationshipTypesDataProvider()
        {
            RelationshipMap = new Dictionary<string, RelationshipType>{
                                                                          {"Identifying", RelationshipType.Identifying},
                                                                          {"Informative", RelationshipType.Informative},
                                                                          {
                                                                              "Non Identifying",
                                                                              RelationshipType.NonIdentifying
                                                                              }
                                                                      };
        }

        /// <summary>
        ///   The map of relationship types.
        /// </summary>
        private static IDictionary<string, RelationshipType> RelationshipMap { get; set; }

        /// <summary>
        ///   The relationship types.
        /// </summary>
        public static IEnumerable RelationshipTypes
        {
            get { return from ship in RelationshipMap select ship.Key; }
        }

        /// <summary>
        ///   Returns RelationshipType by name.
        /// </summary>
        /// <param name = "name">The name.</param>
        /// <returns>The RelationshipType.</returns>
        public static RelationshipType GetTypeByName( string name )
        {
            return RelationshipMap[name];
        }

        /// <summary>
        ///   Returns name by RelationshipType .
        /// </summary>
        /// <param name = "type">The RelationshipType.</param>
        /// <returns>The name.</returns>
        public static string GetNameByType( RelationshipType type)
        {
            return RelationshipMap.FirstOrDefault( pair => pair.Value == type ).Key;
        }
    }
}