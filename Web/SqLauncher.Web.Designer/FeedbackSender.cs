// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   FeedbackSender.cs
//   * Project: SqLauncher.Web.Designer
//   * Description:
//   * Created at 2012 04 15 19:58
//   * Modified at: 2012  04 15  20:26
// / ******************************************************************************/ 

using System;
using System.IO;
using System.Net;
using System.Text;

namespace SqLauncher.Web.Designer
{
    /// <summary>
    ///   Represensts the sender feedback via sqlauncher.com API.
    /// </summary>
    public static class FeedbackSender
    {
        /// <summary>
        ///   The containner for request data exchange.
        /// </summary>
        private class RequestContainer
        {
            /// <summary>
            ///   The request instance.
            /// </summary>
            public HttpWebRequest Request { get; set; }

            /// <summary>
            ///   The feedback  message.
            /// </summary>
            public string Message { get; set; }
        }

        private const string FeedbackApiUrl = "http://sqlauncher.com/api/Feedback.ashx";

        private const string METHOD = "POST";

        private const string MessagePattern =
            @"<feedback>
        <contact>
        {0}
        </contact>
        <email>
        {1}
        </email>
        <subject>
        {2}
        </subject>
        <message>
        {3}
        </message>
    </feedback>";

        /// <summary>
        ///   Sends the feedback data.
        /// </summary>
        /// <param name = "contact">The contact name.</param>
        /// <param name = "email">The contact email.</param>
        /// <param name = "subject">The subject.</param>
        /// <param name = "message">The message.</param>
        public static void Send( string contact, string email, string subject, string message )
        {
            try{
                var request = (HttpWebRequest) WebRequest.Create( FeedbackApiUrl );
                request.Method = METHOD;
                var container = new RequestContainer{
                                                        Request = request,
                                                        Message = string.Format( MessagePattern, contact, email, subject, message )
                                                    };
                request.BeginGetRequestStream( BeginRequest, container );
            } catch{
            } //try
        }

        private static void BeginRequest( IAsyncResult ar )
        {
            var container = (RequestContainer) ar.AsyncState;

            using ( var writer = new StreamWriter( container.Request.EndGetRequestStream( ar ), Encoding.UTF8 ) ){
                writer.Write( container.Message );
            }

            container.Request.BeginGetResponse( BeginGetResponse, container );
        }

        private static void BeginGetResponse( IAsyncResult ar )
        {
            var container = (RequestContainer) ar.AsyncState;

            try{
                container.Request.EndGetResponse( ar );
            } catch{
            } //try
        }
    }
}