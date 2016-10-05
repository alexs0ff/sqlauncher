using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SqLauncher.Web.Examination
{
    public class TestNotifyChanged:INotifyPropertyChanged
    {
        private double _test;

        public double Test
        {
            get { return _test; }
            set
            {
                _test = value;
                OnPropertyChanged(new PropertyChangedEventArgs( "Test" ));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged( PropertyChangedEventArgs e )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if ( handler != null ){
                handler( this, e );
            }
        }
    }
}
