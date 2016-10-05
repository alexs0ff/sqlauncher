// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RibbonList.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 09 25 10:46 AM
//   * Modified at: 2011  09 25  10:55 AM
// / ******************************************************************************/ 

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SqLauncher.Web.Ribbon
{
    public class RibbonList : ListBox
    {
        public RibbonList()
        {
            this.SelectionChanged += RibbonList_SelectionChanged;
            this.MouseLeave += RibbonList_MouseLeave;
            this.Loaded += RibbonList_Loaded;
        }

        private void RibbonList_Loaded( object sender, RoutedEventArgs e )
        {
            foreach ( RibbonListItem item in this.Items ){
                item.hAlignment = hAlignment;
                item.vAlignment = vAlignment;
            }
        }

        private void RibbonList_MouseLeave( object sender, MouseEventArgs e )
        {
            if ( AutoHide ){
                this.Hide();
            }
        }

        private void RibbonList_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            RibbonListItem item = SelectedItem as RibbonListItem;
            if ( this.SelectedIndex != -1 && item != null ){
                item.RaiseOnClick();
            }
        }

        private void Hide()
        {
            this.SelectedIndex = -1;
            if ( _popup != null ){
                _popup.IsOpen = false;
            }
        }

        #region Events

        #endregion

        #region Fields

        private Popup _popup;

        public Popup Popup
        {
            get { return _popup; }
            internal set { _popup = value; }
        }

        private bool _autoHide = true;

        public bool AutoHide
        {
            get { return _autoHide; }
            set { _autoHide = value; }
        }

        private VerticalAlignment _vAlignment = VerticalAlignment.Center;

        public VerticalAlignment vAlignment
        {
            get { return _vAlignment; }
            set { _vAlignment = value; }
        }

        private HorizontalAlignment _hAlignment = HorizontalAlignment.Center;

        public HorizontalAlignment hAlignment
        {
            get { return _hAlignment; }
            set { _hAlignment = value; }
        }

        #endregion
    }
}