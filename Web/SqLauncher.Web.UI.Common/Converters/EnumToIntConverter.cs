// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EnumToIntConverter.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description: http://charlass.wordpress.com/2009/07/29/binding-enums-to-a-combobbox-in-silverlight/
//   * Created at 2012 02 26 18:26
//   * Modified at: 2012  02 26  18:28
// / ******************************************************************************/ 

using System;
using System.Globalization;
using System.Windows.Data;

namespace SqLauncher.Web.UI.Common.Converters
{
    /// <summary>
    ///   The converter for enum to int conversations.
    /// </summary>
    public class EnumToIntConverter : IValueConverter
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
            if ( value == null ){
                return null;
            } //if

            // Note: as pointed out by Martin in the comments on this answer, this line
            // depends on the enum values being sequentially ordered from 0 onward,
            // since combobox indices are done that way. A more general solution would
            // probably look up where in the GetValues array our value variable
            // appears, then return that index.
            return (int) value;
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
            return Enum.Parse( targetType, value.ToString(), true );
        }
    }
}