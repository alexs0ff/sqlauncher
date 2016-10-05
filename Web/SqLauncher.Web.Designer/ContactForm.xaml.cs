using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SqLauncher.Web.Designer
{
    public partial class ContactForm : ChildWindow
    {
        public ContactForm()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty ContactNameProperty =
            DependencyProperty.Register( "ContactName", typeof ( string ), typeof ( ContactForm ),
                                                        new PropertyMetadata( OnContactNameChanged ) );

        private static void OnContactNameChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var form = d as ContactForm;

            if ( form != null ){
                form.ContactName = (string) e.NewValue;
            }
        }

        public string ContactName
        {
            get { return (string) GetValue( ContactNameProperty ); }
            set { SetValue( ContactNameProperty, value ); }
        }

        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register( "Email", typeof ( string ), typeof ( ContactForm ),
                                                        new PropertyMetadata( OnEmailChanged ) );

        private static void OnEmailChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var form = d as ContactForm;

            if (form != null)
            {
                form.Email = (string)e.NewValue;
            }
        }

        public string Email
        {
            get { return (string) GetValue( EmailProperty ); }
            set { SetValue( EmailProperty, value ); }
        }

        public static readonly DependencyProperty SubjectProperty =
            DependencyProperty.Register( "Subject", typeof ( string ), typeof ( ContactForm ),
                                                        new PropertyMetadata( OnSubjectChanged ) );

        private static void OnSubjectChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var form = d as ContactForm;

            if (form != null)
            {
                form.Subject = (string)e.NewValue;
            }
        }

        public string Subject
        {
            get { return (string) GetValue( SubjectProperty ); }
            set { SetValue( SubjectProperty, value ); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register( "Message", typeof ( string ), typeof ( ContactForm ),
                                                        new PropertyMetadata( OnMessageChanged ) );

        private static void OnMessageChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var form = d as ContactForm;

            if (form != null)
            {
                form.Message = (string)e.NewValue;
            }
        }

        public string Message
        {
            get { return (string) GetValue( MessageProperty ); }
            set { SetValue( MessageProperty, value ); }
        }

        private void ButtonCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Send feedback 

        private void ButtonSendClick( object sender, RoutedEventArgs e )
        {
            FeedbackSender.Send( ContactName, Email, Subject, Message );
            Close();
        }

        #endregion Send feedback

    }
}
