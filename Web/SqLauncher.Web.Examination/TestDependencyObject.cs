using System;
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
    public class TestDependencyObject:DependencyObject
    {
        public static readonly DependencyProperty TestProperty =
            DependencyProperty.Register( "Test", typeof ( double ), typeof ( TestDependencyObject ),
                                                        new PropertyMetadata( default( double ),new PropertyChangedCallback(TestPropertyChanged) ) );

        private static void TestPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            TestDependencyObject obj = d as TestDependencyObject;
            obj.Test = (double)e.NewValue;
        }

        public double Test
        {
            get { return (double) GetValue( TestProperty ); }
            set { SetValue( TestProperty, value ); }
        }
    }
}
