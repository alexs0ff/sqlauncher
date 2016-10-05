// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EnumToIEnumerableConverter.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description: http://charlass.wordpress.com/2009/07/29/binding-enums-to-a-combobbox-in-silverlight/
//   * Created at 2012 02 26 18:30
//   * Modified at: 2012  02 26  18:37
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace SqLauncher.Web.UI.Common.Converters
{
    /// <summary>
    ///   The converter for enum to displayable enumeration..
    /// </summary>
    public class EnumToIEnumerableConverter : IValueConverter
    {
        /// <summary>
        ///   The cashe of used values.
        /// </summary>
        private readonly Dictionary<Type, List<object>> _cache = new Dictionary<Type, List<object>>();

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
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value == null ){
                return null;
            } //if

            var type = value.GetType();
            if ( !_cache.ContainsKey( type ) ){
                var fields = type.GetFields().Where( field => field.IsLiteral );
                var values = new List<object>();
                foreach ( var field in fields ){
                    var a = (DescriptionAttribute[]) field.GetCustomAttributes( typeof ( DescriptionAttribute ), false );
                    if ( a != null && a.Length > 0 ){
                        values.Add( a[0].Description );
                    }
                    else{
                        values.Add( field.GetValue( value ) );
                    }
                }
                _cache[type] = values;
            }

            return _cache[type];
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
        public object ConvertBack( object value, Type targetType, object parameter,
                                   System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}