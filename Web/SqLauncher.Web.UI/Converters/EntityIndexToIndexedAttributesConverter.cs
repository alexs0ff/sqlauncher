// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityIndexToIndexedAttributesConverter.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 02 25 20:11
//   * Modified at: 2012  02 25  20:13
// / ******************************************************************************/ 

using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Converters
{
    /// <summary>
    ///   Converts the entity index instance to collection of indexed attributes.
    /// </summary>
    public class EntityIndexToIndexedAttributesConverter : IValueConverter
    {
        /// <summary>
        ///   Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <returns>
        ///   The value to be passed to the target dependency property.
        /// </returns>
        /// <param name = "value">The source data being passed to the target.</param>
        /// <param name = "targetType">The <see cref = "T:System.Type" /> of data expected by the target dependency property.</param>
        /// <param name = "parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name = "culture">The culture of the conversion.</param>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var index = value as EntityIndex;
            if ( index ==  null){
                return null;
            } //if

            var indexedAttributes = new Collection<IndexedAttributeProxy>();

            foreach ( var attribute in index.Parent.Attributes ){
                var atrOfIndex =
                    index.Attributes.FirstOrDefault(
                        indexAttribute => indexAttribute.Attribute.InnerId == attribute.InnerId );

                indexedAttributes.Add( new IndexedAttributeProxy{
                                                                      Attribute = attribute,
                                                                      Indexed = (atrOfIndex!=null),
                                                                      IndexAttribute = atrOfIndex
                                                                  } );
            } //foreach

            return indexedAttributes;
        }

        /// <summary>
        ///   Modifies the target data before passing it to the source object.  This method is called only in <see
        ///    cref = "F:System.Windows.Data.BindingMode.TwoWay" /> bindings.
        /// </summary>
        /// <returns>
        ///   The value to be passed to the source object.
        /// </returns>
        /// <param name = "value">The target data being passed to the source.</param>
        /// <param name = "targetType">The <see cref = "T:System.Type" /> of data expected by the source object.</param>
        /// <param name = "parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name = "culture">The culture of the conversion.</param>
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}