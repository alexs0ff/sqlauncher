// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   App.xaml.cs
//   * Project: SqLauncher.Web.Examination
//   * Description:
//   * Created at 2011 08 16 8:27 PM
//   * Modified at: 2011  08 17  8:58 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.Examination
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.ApplicationStartup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void ApplicationStartup( object sender, StartupEventArgs e )
        {
            this.RootVisual = new MainPage();
        }

        private void Application_Exit( object sender, EventArgs e )
        {
        }

        private void Application_UnhandledException( object sender, ApplicationUnhandledExceptionEventArgs e )
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if ( !System.Diagnostics.Debugger.IsAttached ){
                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke( delegate { ReportErrorToDOM( e ); } );
            }
        }

        private void ReportErrorToDOM( ApplicationUnhandledExceptionEventArgs e )
        {
            try{
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace( '"', '\'' ).Replace( "\r\n", @"\n" );

                System.Windows.Browser.HtmlPage.Window.Eval(
                    "throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");" );
            } catch ( Exception ){
            }
        }
    }
}