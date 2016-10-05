// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SerializerManager.cs
//   * Project: SqLauncher.Web.Designer
//   * Description:
//   * Created at 2012 01 08 13:36
//   * Modified at: 2012  01 08  18:22
// / ******************************************************************************/ 

using System.IO;
using System.Text;

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Common.Encodings;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Designer
{
    /// <summary>
    ///   The manager
    /// </summary>
    public class SerializerManager
    {
        /// <summary>
        ///   The encoding.
        /// </summary>
        private readonly Encoding _encoding;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Designer.SerializerManager" /> class.
        /// </summary>
        public SerializerManager( ISerializerDialog serializerDialog, ContainerWiring wiring )
        {
            SerializerDialog = serializerDialog;
            Wiring = wiring;

            _encoding = Encoding.UTF8;
            SerializerDialog.Serializing += SerializerDialogSerializing;
            SerializerDialog.Deserializing += SerializerDialogDeserializing;
        }

        /// <summary>
        ///   Occurs when user pick up file to load.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void SerializerDialogDeserializing( object sender, DeserializingEventArgs e )
        {
            var streamReader = new StreamReader( e.Stream, _encoding );
            var xml = streamReader.ReadToEnd();
            //TODO: make helper for checking database type etc SqLite, SQL Server and other 

            var xmlSerializer = Wiring.CreateInstance<IDocumentXmlSerializer>();
            ContainerWiring wiring;
            var document = xmlSerializer.Deserialize( xml, out wiring );
            ApplicationController.Controller.CreateSqLiteModel( wiring, document );
        }

        /// <summary>
        ///   Occurs whe user pick up a file to serializing data.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The serializing event args.</param>
        private void SerializerDialogSerializing( object sender, SerializingEventArgs e )
        {
            var xmlSerializer = Wiring.CreateInstance<IDocumentXmlSerializer>();
            var xml = xmlSerializer.Serialize( DatabaseDocument );

            var writer = new StreamWriter( e.FileStream, _encoding );
            writer.Write( xml );
            writer.Flush();
        }

        /// <summary>
        ///   Saves the database document.
        /// </summary>
        /// <param name = "document">The database document.</param>
        public void SaveDatabaseDocument( DatabaseDocument document )
        {
            DatabaseDocument = document;
            SerializerDialog.ChoiceFileToSave();
        }

        /// <summary>
        ///   Loades the database document.
        /// </summary>
        public void LoadDatabaseDocument()
        {
            SerializerDialog.ChoiceFileToLoad();
        }

        /// <summary>
        ///   The processed document.
        /// </summary>
        private DatabaseDocument DatabaseDocument { get; set; }

        /// <summary>
        ///   The serializer dialog.
        /// </summary>
        public ISerializerDialog SerializerDialog { get; private set; }

        /// <summary>
        ///   The container wiring.
        /// </summary>
        private ContainerWiring Wiring { get; set; }

        /// <summary>
        ///   Stops the serialization process.
        /// </summary>
        public void Stop()
        {
            SerializerDialog.Serializing -= SerializerDialogSerializing;
            SerializerDialog.Deserializing -= SerializerDialogDeserializing;
        }
    }
}