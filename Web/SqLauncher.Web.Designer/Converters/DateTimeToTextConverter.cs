// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DateTimeToTextConverter.cs
//   * Project: SqLauncher.Web.Designer
//   * Description:
//   * Created at 2011 11 29 16:49
//   * Modified at: 2011  11 29  16:53
// / ******************************************************************************/ 

using System;
using System.Globalization;
using System.Windows.Data;

namespace SqLauncher.Web.Designer.Converters
{
    /// <summary>
    ///   The converter from datetime values to text represents.
    /// </summary>
    public class DateTimeToTextConverter : IValueConverter
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
            string result = string.Empty;

            if ( value != null ){
                var val = value as DateTime?;
                if ( val != null ){
                    result = string.Format( "{0} {1}", val.Value.ToShortDateString(), val.Value.ToShortTimeString() );
                }
            }

            return result;
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