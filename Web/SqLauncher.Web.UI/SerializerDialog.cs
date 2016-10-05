// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SerializerDialog.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 01 08 12:28
//   * Modified at: 2012  01 08  17:55
// / ******************************************************************************/ 

using System;
using System.IO;
using System.Windows.Controls;

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   Represents the serializer dialog for save and load operations.
    /// </summary>
    public class SerializerDialog : ISerializerDialog
    {
        /// <summary>
        ///   The pattern for file save dialog.
        /// </summary>
        private const string FilterPattern = "SqLauncher Files (*.sqlr)|*.sqlr|All Files (*.*)|*.*";

        /// <summary>
        ///   Starts the serializing process.
        /// </summary>
        public void ChoiceFileToSave()
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = FilterPattern;
            var showDialog = saveFileDialog.ShowDialog();
            if ( showDialog != null && showDialog.Value ){
                using ( var stream = saveFileDialog.OpenFile() ){
                    RiseSerializing( saveFileDialog.SafeFileName, stream );
                }
            }
        }

        public event EventHandler<SerializingEventArgs> Serializing;

        /// <summary>
        ///   The invocator serializing event handler.
        /// </summary>
        /// <param name = "file"></param>
        /// <param name = "stream"></param>
        public void RiseSerializing( string file, Stream stream )
        {
            EventHandler<SerializingEventArgs> handler = Serializing;
            if ( handler != null ){
                handler( this, new SerializingEventArgs( file, stream ) );
            }
        }

        /// <summary>
        ///   Starts the deserializing process.
        /// </summary>
        public void ChoiceFileToLoad()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = FilterPattern;
            openFileDialog.Multiselect = false;
            var showDialog = openFileDialog.ShowDialog();
            if ( showDialog != null && showDialog.Value ){
                using ( var stream = openFileDialog.File.OpenRead() ){
                    RiseDeserializing( openFileDialog.File.Name, stream );
                }
            }
        }

        public event EventHandler<DeserializingEventArgs> Deserializing;

        /// <summary>
        ///   Rises the Deserializing event.
        /// </summary>
        private void RiseDeserializing( string file, Stream stream )
        {
            EventHandler<DeserializingEventArgs> handler = Deserializing;
            if ( handler != null ){
                handler( this, new DeserializingEventArgs( file, stream ) );
            }
        }
    }
}