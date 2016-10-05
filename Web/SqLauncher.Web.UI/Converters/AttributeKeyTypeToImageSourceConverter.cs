// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AttributeKeyTypeToImageSourceConverter.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 20 11:42
//   * Modified at: 2011  11 20  11:49
// / ******************************************************************************/ 

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Converters
{
    public class AttributeKeyTypeToImageSourceConverter : IValueConverter
    {
        /// <summary>
        ///   The image uri for primary key.
        /// </summary>
        public const string PrimaryKeyImageUri = "/SqLauncher.Web.UI;component/Images/PrimaryKey.png";

        /// <summary>
        /// The image uri for foreign key.
        /// </summary>
        public const string ForeignKeyImageUri = "/SqLauncher.Web.UI;component/Images/ForeignKey.png";

        /// <summary>
        /// The image uri for primary foreign key.
        /// </summary>
        public const string PrimaryForeignKeyImageUri = "/SqLauncher.Web.UI;component/Images/PrimaryForeignKey.png";

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
            BitmapImage result = null;

            if ( value is AttributeKeyType ){
                var keyType = (AttributeKeyType) value;

                switch ( keyType ){
                    case AttributeKeyType.IsKey:
                        result =
                            new BitmapImage( new Uri( PrimaryKeyImageUri,
                                                      UriKind.Relative ) );
                        break;
                    case AttributeKeyType.IsForeignKey:
                        result =
                            new BitmapImage(new Uri(ForeignKeyImageUri,
                                                      UriKind.Relative));
                        break;
                        case AttributeKeyType.IsPrimaryForeignKey:
                        result =
                            new BitmapImage(new Uri(PrimaryForeignKeyImageUri,
                                                      UriKind.Relative));
                        break;
                } //switch
            } //if

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