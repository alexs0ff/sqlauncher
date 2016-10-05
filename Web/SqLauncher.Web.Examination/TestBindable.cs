using System;
using System.Collections.Generic;
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
    public class TestItem:INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RisePropertyChanged( "Name" );
            }
        }

        private int _age;

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                RisePropertyChanged("Age");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class TestBindable:INotifyPropertyChanged
    {
        public ICollection<TestItem> TestItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RisePropertyChanged( string propertyName )
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if ( handler != null ){
                handler( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }
    }
}
