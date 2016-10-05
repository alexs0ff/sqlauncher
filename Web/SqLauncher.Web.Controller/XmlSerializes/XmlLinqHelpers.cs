// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   XmlLinqHelpers.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 01 04 14:39
//   * Modified at: 2012  01 04  14:43
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SqLauncher.Web.Controller.XmlSerializes
{
    /// <summary>
    ///   The helper for xml to linq purposes.
    /// </summary>
    public static class XmlLinqHelpers
    {
        /// <summary>
        ///   Gets the value attribute by attribute name or null.
        /// </summary>
        /// <param name = "attributes">The attributes.</param>
        /// <param name = "attributeName">The atribute name.</param>
        /// <returns>The value or null.</returns>
        public static string GetAttributeValue( this IEnumerable<XAttribute> attributes, string attributeName )
        {
            string result = null;

            var attribute =
                attributes.FirstOrDefault(
                    atr => StringComparer.OrdinalIgnoreCase.Compare( atr.Name.LocalName, attributeName ) == 0 );

            if ( attribute != null ){
                result = attribute.Value;
            } //if

            return result;
        }

        /// <summary>
        /// Gets x element by name.
        /// </summary>
        /// <param name="elements">The elements collection.</param>
        /// <param name="elementName">The element name.</param>
        /// <returns>The element or null. </returns>
        public static XElement GetElementByName(this IEnumerable<XElement> elements, string elementName )
        {
            return
                elements.FirstOrDefault(
                    el => StringComparer.OrdinalIgnoreCase.Compare( el.Name.LocalName, elementName ) == 0 );
        }
    }
}