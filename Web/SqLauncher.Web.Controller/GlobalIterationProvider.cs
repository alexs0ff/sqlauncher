// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   GlobalIterationProvider.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 12 23 23:15
//   * Modified at: 2011  12 23  23:20
// / ******************************************************************************/ 

using System.Windows;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   The singleton for global ui iteractions.
    /// </summary>
    public class GlobalIterationProvider : DependencyObject
    {
        #region CanInsert 

        public static readonly DependencyProperty CanInsertProperty =
            DependencyProperty.Register( "CanInsert", typeof ( bool ), typeof ( GlobalIterationProvider ),
                                         new PropertyMetadata( CanInsertChanged ) );

        private static void CanInsertChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var obj = (GlobalIterationProvider) d;
            obj.CanInsert = (bool) e.NewValue;
        }

        /// <summary>
        ///   Indicates that we can insert a data from clipboard.
        /// </summary>
        public bool CanInsert
        {
            get { return (bool) GetValue( CanInsertProperty ); }
            set { SetValue( CanInsertProperty, value ); }
        }

        #endregion CanInsert
    }
}