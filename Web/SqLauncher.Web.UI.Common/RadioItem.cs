// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RadioItem.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 16 7:21 PM
//   * Modified at: 2011  10 16  7:21 PM
// / ******************************************************************************/ 

using System.ComponentModel;

namespace SqLauncher.Web.UI.Common
{
    /// <summary>
    ///   The radio item from list.
    /// </summary>
    public class RadioItem : INotifyPropertyChanged
    {
        /// <summary>
        ///   The value.
        /// </summary>
        public object Value { get; set; }

        private bool _isChecked;

        /// <summary>
        ///   Is checked property.
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RisePropertyChanged( new PropertyChangedEventArgs( "IsChecked" ) );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RisePropertyChanged( PropertyChangedEventArgs e )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if ( handler != null ){
                handler( this, e );
            }
        }
    }
}